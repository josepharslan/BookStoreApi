using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Abstract
{
    public interface INewsletterService
    {
        Task QueueCampaignAsync(string subject, string htmlBody, CancellationToken ct = default);
        Task QueueTestAsync(string to, string subject, string htmlBody, CancellationToken ct = default);
    }
}
