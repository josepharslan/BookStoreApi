using BookStore.WebUI.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.WebUI.ViewComponents
{
    public class UIRandomBook : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UIRandomBook(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                var productCountResponse = await client.GetAsync("https://localhost:7287/api/Products/ProductCount");
                if (!productCountResponse.IsSuccessStatusCode)
                {
                    return View(new ResultProductDto());
                }

                var jsonData = await productCountResponse.Content.ReadAsStringAsync();
                var totalCount = JsonConvert.DeserializeObject<int>(jsonData);

                if (totalCount <= 0)
                {
                    return View(new ResultProductDto());
                }

                Random rand = new Random();
                HttpResponseMessage randomProductResponse;
                ResultProductDto? product = null;

                do
                {
                    var randomBookId = rand.Next(1, totalCount + 1);
                    randomProductResponse = await client.GetAsync("https://localhost:7287/api/Products/GetProduct?id=" + randomBookId);

                    if (randomProductResponse.IsSuccessStatusCode)
                    {
                        var productJson = await randomProductResponse.Content.ReadAsStringAsync();
                        product = JsonConvert.DeserializeObject<ResultProductDto>(productJson);
                    }

                } while (product == null);

                return View(product);
            }
            catch (Exception)
            {
                return View(new ResultProductDto());
            }
        }

    }
}

