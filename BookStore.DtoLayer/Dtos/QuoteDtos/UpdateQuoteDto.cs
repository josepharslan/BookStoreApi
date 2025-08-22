using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DtoLayer.Dtos.QuoteDtos
{
    public class UpdateQuoteDto
    {
        public int QuoteId { get; set; }
        public string QuoteName { get; set; }
    }
}
