using BookStore.DtoLayer.Dtos.SubscriberDto;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class SubscriberController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SubscriberController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7287/api/Subscribers");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<List<SubscriberListDto>>(jsonData);
                return View(values);
            }
            return View();
        }
        [HttpGet]
        public IActionResult AddNewsletter()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddNewsletter(SendEmailDto dto, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(dto);

            var client = _httpClientFactory.CreateClient();

            var jsonData = JsonConvert.SerializeObject(dto);
            using var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://localhost:7287/api/Newsletters/send", content, ct);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("Index");

            var error = await response.Content.ReadAsStringAsync(ct);
            ModelState.AddModelError(string.Empty, $"API hatası: {(int)response.StatusCode} - {error}");
            return View(dto);
        }

    }
}
