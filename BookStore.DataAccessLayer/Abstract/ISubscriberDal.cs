using BookStore.EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccessLayer.Abstract
{
    public sealed record MonthlyCountDto(int Year, int Month, int Count);
    public interface ISubscriberDal
    {
        Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default);
        Task AddAsync(Subscriber sub, CancellationToken ct = default);
        Task<List<Subscriber>> GetActiveAsync(CancellationToken ct = default);
        Task<List<Subscriber>> GetAll();
        Task<IReadOnlyList<MonthlyCountDto>> CountByMonthAsync(int year);
    }
}
