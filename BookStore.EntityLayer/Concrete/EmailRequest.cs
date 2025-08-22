using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.EntityLayer.Concrete
{
    public sealed class EmailRequest
    {
        public string To { get; init; } = "";
        public string Subject { get; init; } = "";
        public string HtmlBody { get; init; } = "";
        public string? TextBody { get; init; }
    }
}
