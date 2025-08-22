using BookStore.WebUI.Dtos.CategoryDtos;
using BookStore.WebUI.Dtos.ProductDtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.WebUI.ViewComponents
{
    public class UIPopularBooks : ViewComponent
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UIPopularBooks(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Get categories
                var categoriesResponse = await client.GetAsync("https://localhost:7287/api/Categories");
                if (categoriesResponse.IsSuccessStatusCode)
                {
                    var categoriesJson = await categoriesResponse.Content.ReadAsStringAsync();
                    ViewBag.Categories = JsonConvert.DeserializeObject<List<ResultCategoryDto>>(categoriesJson);
                }
                else
                {
                    ViewBag.Categories = new List<ResultCategoryDto>();
                }

                // Get products
                var productsResponse = await client.GetAsync("https://localhost:7287/api/Products");
                if (productsResponse.IsSuccessStatusCode)
                {
                    var productsJson = await productsResponse.Content.ReadAsStringAsync();
                    var allProducts = JsonConvert.DeserializeObject<List<ResultProductDto>>(productsJson);

                    ViewBag.AllProducts = allProducts;
                    ViewBag.ProductsByCategory = allProducts
                        .GroupBy(p => p.CategoryId)
                        .ToDictionary(g => g.Key, g => g.ToList());
                }
                else
                {
                    ViewBag.AllProducts = new List<ResultProductDto>();
                    ViewBag.ProductsByCategory = new Dictionary<int, List<ResultProductDto>>();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Categories = new List<ResultCategoryDto>();
                ViewBag.AllProducts = new List<ResultProductDto>();
                ViewBag.ProductsByCategory = new Dictionary<int, List<ResultProductDto>>();
            }

            return View();
        }
    }
}
