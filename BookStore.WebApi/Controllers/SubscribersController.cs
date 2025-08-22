using BookStore.DataAccessLayer.Abstract;
using BookStore.EntityLayer.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class SubscribersController : ControllerBase
    {
        private readonly ISubscriberDal _subscriberDal;

        public SubscribersController(ISubscriberDal subscriberDal)
        {
            _subscriberDal = subscriberDal;
        }

        public class SubscribeDto
        {
            [Required, EmailAddress] public string Email { get; set; } = default!;
        }

        [HttpPost]
        public async Task<IActionResult> Subscribe(SubscribeDto dto)
        {
            var email = dto.Email.Trim().ToLowerInvariant();
            if (await _subscriberDal.ExistsByEmailAsync(email))
            {
                return Conflict(new { message = "zaten_abone" });
            }
            var sub = new Subscriber
            {
                SubcriberMail = email,
                IsActive = true
            };

            await _subscriberDal.AddAsync(sub);
            return Created($"/api/subscribers/{sub.SubscriberId}", new { id = sub.SubscriberId });
        }
        [HttpGet]
        public async Task<IActionResult> SubscriberList()
        {
            var value = await _subscriberDal.GetActiveAsync();
            return Ok(value.ToList());
        }
        [HttpGet("Monthly")]
        public async Task<IActionResult> GetMonthly(int? year)
        {
            var y = year ?? DateTime.UtcNow.Year;

            var items = await _subscriberDal.CountByMonthAsync(y);

            var tr = new System.Globalization.CultureInfo("tr-TR");

            var result = items.Select(i => new
            {
                i.Year,
                i.Month,
                MonthName = tr.DateTimeFormat.GetMonthName(i.Month),
                i.Count
            });

            return Ok(new { Year = y, Items = result });
        }
    }
}
