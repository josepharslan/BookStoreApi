using BookStore.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Abstract
{
    public interface IEmailQueue
    {
        ValueTask EnqueueAsync(EmailRequest msg, CancellationToken ct = default);
        IAsyncEnumerable<EmailRequest> ReadAllAsync(CancellationToken ct = default);

    }
}
