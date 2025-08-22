using BookStore.DtoLayer.Dtos.QuoteDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.WebUI.ViewComponents
{
    public class UIQuotes : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UIQuotes(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var client = _httpClientFactory.CreateClient();
            var responseMessage = await client.GetAsync("https://localhost:7287/api/Quotes/GetLast");
            if (responseMessage.IsSuccessStatusCode)
            {
                var jsonData = await responseMessage.Content.ReadAsStringAsync();
                var values = JsonConvert.DeserializeObject<ResultQuoteDto>(jsonData);
                return View(values);
            }
            return View();
        }
    }
}
