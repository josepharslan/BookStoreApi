using BookStore.BusinessLayer.Abstract;
using BookStore.DataAccessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Concrete
{
    public class NewsletterManager : INewsletterService
    {
        private readonly ISubscriberDal _subscriberDal;
        private readonly IEmailQueue _emailQueue;

        public NewsletterManager(ISubscriberDal subscriberDal, IEmailQueue emailQueue)
        {
            _subscriberDal = subscriberDal;
            _emailQueue = emailQueue;
        }

        public async Task QueueCampaignAsync(string subject, string htmlBody, CancellationToken ct = default)
        {
            var list = await _subscriberDal.GetActiveAsync(ct);

            foreach (var s in list)
            {
                var personalizedHtml = (htmlBody ?? string.Empty)
                    .Replace("{{email}}", System.Net.WebUtility.HtmlEncode(s.SubcriberMail));

                await _emailQueue.EnqueueAsync(new EmailRequest
                {
                    To = s.SubcriberMail,
                    Subject = subject,
                    HtmlBody = personalizedHtml
                }, ct);
            }
        }

        public Task QueueTestAsync(string to, string subject, string htmlBody, CancellationToken ct = default)
        => _emailQueue.EnqueueAsync(new EmailRequest
        {
            To = to,
            Subject = subject,
            HtmlBody = htmlBody
        }, ct).AsTask();
    }
}
