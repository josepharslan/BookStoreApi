using BookStore.DataAccessLayer.Abstract;
using BookStore.DataAccessLayer.Context;
using BookStore.DataAccessLayer.Repositories;
using BookStore.EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DataAccessLayer.EntityFramework
{
    public class EfSubscriberDal : ISubscriberDal
    {
        private readonly BookStoreContext _context;
        public EfSubscriberDal(BookStoreContext context) => _context = context;

        public Task<bool> ExistsByEmailAsync(string email, CancellationToken ct = default)
            => _context.Subscribers.AsNoTracking().AnyAsync(x => x.SubcriberMail == email, ct);

        public async Task AddAsync(Subscriber sub, CancellationToken ct = default)
        {
            await _context.Subscribers.AddAsync(sub, ct);
            await _context.SaveChangesAsync(ct);
        }

        public Task<List<Subscriber>> GetActiveAsync(CancellationToken ct = default)
            => _context.Subscribers.AsNoTracking()
                .Where(s => s.IsActive)
                .ToListAsync(ct);

        public Task<List<Subscriber>> GetAll()
            => _context.Subscribers.ToListAsync();

        public async Task<IReadOnlyList<MonthlyCountDto>> CountByMonthAsync(int year)
        {
            var raw = await _context.Subscribers
                .Where(s => s.CreatedDate.Year == year)
                .GroupBy(s => new { s.CreatedDate.Year, s.CreatedDate.Month })
                .Select(g => new MonthlyCountDto(g.Key.Year, g.Key.Month, g.Count()))
                .ToListAsync();

            var map = raw.ToDictionary(x => x.Month, x => x.Count);
            var full = Enumerable.Range(1, 12)
                .Select(m => new MonthlyCountDto(year, m, map.TryGetValue(m, out var c) ? c : 0))
                .ToList();

            return full;
        }
    }
}

