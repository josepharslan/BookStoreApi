using BookStore.DtoLayer.Dtos.SubscriberDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.EntityLayer.Concrete
{
    public sealed class DashboardStats
    {
        public int ProductTotal { get; set; }
        public int SubscriberTotal { get; set; }
        public int SubscriberActive { get; set; }
        public int QuoteTotal { get; set; }
        public int TopWriterBookCount { get; set; }
        public string TopWriterName { get; set; }
        public IReadOnlyList<Subscriber> RecentSubscribers { get; set; }
        public IReadOnlyList<Product> LastProducts { get; set; }
        public Product MostExpensiveProduct { get; set; }
        public Product ProductHasLowestStock { get; set; }
        public Category CategoryHasMostBook { get; set; }
        public Product WriterHasMostBooks { get; set; }
        public string MostSubcriberMonth { get; set; }
        public int MostSubcriberMonthCount { get; set; }
        public int Year { get; set; }
        public List<MonthlySubscriberItemDto> MonthlySubscriber { get; set; } = new();
    }
}
