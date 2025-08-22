
using BookStore.BusinessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using System.Net.Mail;

namespace BookStore.WebApi.Services
{
    public sealed class EmailBackgroundService : BackgroundService
    {
        private readonly IEmailQueue _queue;
        private readonly IEmailService _sender;
        private readonly ILogger<EmailBackgroundService> _logger;
        private readonly int _ratePerMinute;
        public EmailBackgroundService(
          IEmailQueue queue,
          IEmailService sender,
          ILogger<EmailBackgroundService> logger)
        {
            _queue = queue ?? throw new ArgumentNullException(nameof(queue));
            _sender = sender ?? throw new ArgumentNullException(nameof(sender));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var envRate = Environment.GetEnvironmentVariable("EMAIL_RATE_LIMIT_PER_MINUTE");
            _ratePerMinute = int.TryParse(envRate, out var v) && v > 0 ? v : 60;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("EmailBackgroundService started. Rate: {Rate}/min", _ratePerMinute);

            var delayMs = (int)Math.Ceiling(60000.0 / _ratePerMinute);

            await foreach (var msg in _queue.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await SendWithRetryAsync(msg, stoppingToken);

                    if (delayMs > 0)
                        await Task.Delay(delayMs, stoppingToken);
                }
                catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "E-posta gönderiminde beklenmedik hata. Alıcı: {To}", msg.To);
                }
            }

            _logger.LogInformation("EmailBackgroundService stopped.");
        }
        private async Task SendWithRetryAsync(EmailRequest msg, CancellationToken ct)
        {
            const int maxAttempts = 3;
            for (var attempt = 1; attempt <= maxAttempts; attempt++)
            {
                ct.ThrowIfCancellationRequested();

                try
                {
                    await _sender.SendAsync(msg, ct);
                    _logger.LogDebug("E-posta gönderildi. Alıcı: {To}", msg.To);
                    return;
                }
                catch (OperationCanceledException) when (ct.IsCancellationRequested)
                {
                    throw;
                }
                catch (Exception ex) when (attempt < maxAttempts)
                {
                    var backoffMs = (int)Math.Pow(2, attempt) * 500; // 500, 1000, 2000ms
                    _logger.LogWarning(ex,
                        "Gönderim denemesi {Attempt}/{Max}. {Delay}ms sonra tekrar denenecek. Alıcı: {To}",
                        attempt, maxAttempts, backoffMs, msg.To);

                    await Task.Delay(backoffMs, ct);
                }
            }

            // Son deneme da başarısızsa exception yukarıya çıksın (ExecuteAsync tarafından loglanır)
            await _sender.SendAsync(msg, ct);
        }

    }
}
