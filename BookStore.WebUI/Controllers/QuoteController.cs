using BookStore.DtoLayer.Dtos.QuoteDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class QuoteController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public QuoteController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> QuoteList()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7287/api/Quotes");
            var jsonData = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ResultQuoteDto>>(jsonData);
            return View(values);
        }
        public IActionResult CreateQuote()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateQuote(CreateQuoteDto dto)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonData = JsonConvert.SerializeObject(dto);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var responseMessage = await client.PostAsync("https://localhost:7287/api/Quotes", content);
            if (responseMessage.IsSuccessStatusCode)
            {
                return RedirectToAction("QuoteList");
            }
            return View();
        }
    }
}
