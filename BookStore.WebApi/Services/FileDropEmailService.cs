using BookStore.BusinessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using System.Text;

namespace BookStore.WebApi.Services
{
    public sealed class FileDropEmailService : IEmailService
    {
        private readonly string _dropPath;
        public FileDropEmailService()
        {
            _dropPath = Environment.GetEnvironmentVariable("EMAIL_DROP_PATH")
                ?? Path.Combine(AppContext.BaseDirectory, "mail-drop");
            Directory.CreateDirectory(_dropPath);
        }

        public Task SendAsync(EmailRequest message, CancellationToken ct = default)
        {
            var safeTo = Sanitize(message.To);
            var fileName = $"{DateTime.UtcNow:yyyyMMdd_HHmmss_fff}-{safeTo}.html";
            var fullPath = Path.Combine(_dropPath, fileName);

            var sb = new StringBuilder();
            sb.AppendLine($"<h3>Kime: {System.Net.WebUtility.HtmlEncode(message.To)}</h3>");
            sb.AppendLine($"<h4>Konu: {System.Net.WebUtility.HtmlEncode(message.Subject)}</h4>");
            sb.AppendLine("<hr/>");
            sb.AppendLine(message.HtmlBody ?? string.Empty);

            return File.WriteAllTextAsync(fullPath, sb.ToString(), ct);
        }
        private static string Sanitize(string s)
            => new string((s ?? "").Where(ch => char.IsLetterOrDigit(ch) || ch == '-' || ch == '_').ToArray());

    }
}
