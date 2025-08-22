using BookStore.BusinessLayer.Abstract;
using BookStore.DataAccessLayer.Abstract;
using BookStore.DtoLayer.Dtos.SubscriberDto;
using BookStore.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.BusinessLayer.Concrete
{
    public sealed class DashboardManager : IDashboardService
    {
        private readonly IProductService _productService;
        private readonly IQuoteService _quoteService;
        private readonly ICategoryService _categoryService;
        private readonly ISubscriberDal _subscriberDal;

        public DashboardManager(IProductService productService, IQuoteService quoteService, ICategoryService categoryService, ISubscriberDal subscriberDal)
        {
            _productService = productService;
            _quoteService = quoteService;
            _categoryService = categoryService;
            _subscriberDal = subscriberDal;
        }

        public async Task<DashboardStats> GetAsync(int? year)
        {
            int recentTake = 5;
            var y = year ?? DateTime.UtcNow.Year;
            var tr = new System.Globalization.CultureInfo("tr-Tr");
            var monthlyRaw = await _subscriberDal.CountByMonthAsync(y);

            var productTotalTask = _productService.TGetProductCount();
            var subscribeTotalTask = _subscriberDal.GetAll().Result.Count();
            var sucbriberActiveTask = _subscriberDal.GetActiveAsync().Result;
            var quoteTotalTask = _quoteService.TGetAll().Count();

            var recentSubscribersTask = _subscriberDal.GetAll().Result.OrderByDescending(x => x.SubscriberId).Take(recentTake);
            var lastProductsTask = _productService.TGetAll().OrderByDescending(x => x.ProductId).Take(recentTake);

            var mostExpensiveProductTask = _productService.TGetAll().OrderByDescending(x => x.ProductPrice).FirstOrDefault();
            var lowestStockProductTask = _productService.TGetAll().OrderBy(x => x.ProductStock).FirstOrDefault();
            var categoryWithMostTask = _categoryService.TGetAll().GroupJoin(_productService.TGetAll(),
                c => c.CategoryId, p => p.CategoryId,
                (c, ps) => new { Category = c, Count = ps.Count() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Category.CategoryId)
                .Select(x => x.Category)
                .FirstOrDefault();
            var writerHasMostBookTask = _productService.TGetAll()
                .Where(p => !string.IsNullOrWhiteSpace(p.ProductWriter))
                .GroupBy(p => p.ProductWriter)
                .Select(g => new { Writer = g.Key, Count = g.Count(), Sample = g.FirstOrDefault() })
                .OrderByDescending(x => x.Count)
                .ThenBy(x => x.Writer)
                .FirstOrDefault();

            var monthly = monthlyRaw
                .Select(x => new MonthlySubscriberItemDto
                {
                    Month = x.Month,
                    MonthName = tr.DateTimeFormat.GetMonthName(x.Month),
                    Count = x.Count
                })
                .ToList();
            var mostSubcriberMonthTask = monthly.OrderByDescending(x => x.Count).FirstOrDefault();
            return new DashboardStats
            {
                ProductTotal = productTotalTask,
                SubscriberTotal = subscribeTotalTask,
                SubscriberActive = sucbriberActiveTask.Count,
                QuoteTotal = quoteTotalTask,

                RecentSubscribers = recentSubscribersTask.ToList(),
                LastProducts = lastProductsTask.ToList(),

                MostExpensiveProduct = mostExpensiveProductTask,
                ProductHasLowestStock = lowestStockProductTask,
                CategoryHasMostBook = categoryWithMostTask,
                TopWriterName = writerHasMostBookTask.Writer,
                TopWriterBookCount = writerHasMostBookTask.Count,
                Year = y,
                MonthlySubscriber = monthly,
                MostSubcriberMonth = mostSubcriberMonthTask.MonthName,
                MostSubcriberMonthCount = mostSubcriberMonthTask.Count,
            };
        }
    }
}
