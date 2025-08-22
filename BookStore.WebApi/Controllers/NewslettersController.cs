using BookStore.BusinessLayer.Abstract;
using BookStore.DataAccessLayer.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookStore.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewslettersController : ControllerBase
    {
        private readonly INewsletterService _newsletterService;

        public NewslettersController(INewsletterService newsletterService)
        {
            _newsletterService = newsletterService;
        }
        public class CampaignDto
        {
            [Required, MaxLength(180)] public string Subject { get; set; } = "";
            [Required] public string Body { get; set; } = "";
        }

        // POST /api/newsletter/send
        [HttpPost("send")]
        public async Task<IActionResult> Send([FromBody] CampaignDto dto)
        {
            await _newsletterService.QueueCampaignAsync(dto.Subject, dto.Body);
            return Accepted(new { status = "Başarılı" });
        }

        // POST /api/newsletter/test?to=...
        [HttpPost("Test")]
        public async Task<IActionResult> Test([FromQuery][EmailAddress] string to, [FromBody] CampaignDto dto)
        {
            await _newsletterService.QueueTestAsync(to, dto.Subject, dto.Body);
            return Accepted(new { status = "basarili" });
        }
    }
}
