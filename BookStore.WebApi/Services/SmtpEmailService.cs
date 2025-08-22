using BookStore.BusinessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using MimeKit;
using MailKit.Net.Smtp;
using System.Text.RegularExpressions;
using MailKit.Security;

namespace BookStore.WebApi.Services
{
    public sealed class SmtpEmailService : IEmailService
    {
        private readonly ILogger<SmtpEmailService> _logger;

        private readonly string _host;
        private readonly int _port;
        private readonly SecureSocketOptions _security;
        private readonly string _user;
        private readonly string _pass;
        private readonly string _from;
        private readonly string _name;

        public SmtpEmailService(ILogger<SmtpEmailService> logger)
        {
            _logger = logger;

            _host = Environment.GetEnvironmentVariable("SMTP_HOST") ?? "";
            _port = int.TryParse(Environment.GetEnvironmentVariable("SMTP_PORT"), out var p) ? p : 587;

            var sec = (Environment.GetEnvironmentVariable("SMTP_SECURITY") ?? "starttls").ToLowerInvariant();
            _security = sec switch
            {
                "ssl" => SecureSocketOptions.SslOnConnect, // 465
                "starttls" => SecureSocketOptions.StartTls,     // 587
                "none" => SecureSocketOptions.None,
                _ => SecureSocketOptions.StartTls
            };

            _user = Environment.GetEnvironmentVariable("SMTP_USERNAME") ?? "";
            _pass = Environment.GetEnvironmentVariable("SMTP_PASSWORD") ?? "";
            _from = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS") ?? "";
            _name = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME") ?? "No-Reply";

            if (string.IsNullOrWhiteSpace(_host) || string.IsNullOrWhiteSpace(_from))
            {
                _logger.LogWarning("SMTP konfigürasyonu eksik olabilir. SMTP_HOST='{Host}', EMAIL_FROM_ADDRESS='{From}'", _host, _from);
            }
        }

        public async Task SendAsync(EmailRequest message, CancellationToken ct = default)
        {
            var missing = new List<string>();
            if (string.IsNullOrWhiteSpace(_host)) missing.Add("SMTP_HOST");
            if (_port <= 0) missing.Add("SMTP_PORT");
            if (string.IsNullOrWhiteSpace(_from)) missing.Add("EMAIL_FROM_ADDRESS");

            if (missing.Count > 0)
            {
                var detail = string.Join(", ", missing);
                _logger.LogError("SMTP yanlış/eksik konfigürasyon: {Missing}", detail);
                throw new InvalidOperationException($"SMTP misconfigured. Missing: {detail}");
            }

            var mime = new MimeMessage();
            mime.From.Add(new MailboxAddress(_name, _from));
            mime.To.Add(MailboxAddress.Parse(message.To));
            mime.Subject = message.Subject;

            var body = new BodyBuilder
            {
                HtmlBody = message.HtmlBody,
                TextBody = string.IsNullOrWhiteSpace(message.TextBody)
                           ? System.Text.RegularExpressions.Regex.Replace(message.HtmlBody ?? "", "<.*?>", string.Empty)
                           : message.TextBody
            };
            mime.Body = body.ToMessageBody();

            using var client = new SmtpClient();
            await client.ConnectAsync(_host, _port, _security, ct);

            if (!string.IsNullOrEmpty(_user))
                await client.AuthenticateAsync(_user, _pass, ct);

            await client.SendAsync(mime, ct);
            await client.DisconnectAsync(true, ct);

            _logger.LogInformation("E-posta gönderildi. Alıcı: {To}", message.To);
        }
    }
}

