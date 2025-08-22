using BookStore.BusinessLayer.Abstract;
using BookStore.EntityLayer.Concrete;

namespace BookStore.WebApi.Services
{
    public sealed class SendGridEmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromAddress;
        private readonly string _fromName;

        public SendGridEmailService(string apiKey, string fromAddress, string fromName)
        {
            _apiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY")
                       ?? throw new InvalidOperationException("SENDGRID_API_KEY not set.");
            _fromAddress = Environment.GetEnvironmentVariable("EMAIL_FROM_ADDRESS")
                ?? throw new InvalidOperationException("EMAIL_FROM_ADDRESS not set.");
            _fromName = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME") ?? "No-Reply";
        }

        public Task SendAsync(EmailRequest message, CancellationToken ct = default)
        {
            throw new NotImplementedException();
        }
    }
}
