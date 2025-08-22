using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DtoLayer.Dtos.SubscriberDto
{
    public class SendEmailDto
    {
        public string Subject { get; set; }
        public string Body { get; set; }

        public string TestSendTo { get; set; }
    }
}
