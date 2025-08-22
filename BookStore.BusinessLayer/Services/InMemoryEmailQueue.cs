using BookStore.BusinessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Services
{
    public class InMemoryEmailQueue : IEmailQueue
    {
        private readonly Channel<EmailRequest> _channel;

        public InMemoryEmailQueue(int capacity)
        {
            _channel = Channel.CreateBounded<EmailRequest>(new BoundedChannelOptions(capacity)
            {
                FullMode = BoundedChannelFullMode.Wait,
                SingleReader = false,
                SingleWriter = false,
            });
        }

        public ValueTask EnqueueAsync(EmailRequest msg, CancellationToken ct = default)
            => _channel.Writer.WriteAsync(msg, ct);

        public IAsyncEnumerable<EmailRequest> ReadAllAsync(CancellationToken ct = default)
            => _channel.Reader.ReadAllAsync(ct);

    }
}
