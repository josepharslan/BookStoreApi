using BookStore.EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace BookStore.WebUI.Controllers
{
    public class AdminController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AdminController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Dashboard()
        {
            var client = _httpClientFactory.CreateClient();
            var y = DateTime.Now.Year;
            var value = await client.GetFromJsonAsync<DashboardStats>("https://localhost:7287/api/Dashboard");
            if (value is null)
            {
                ModelState.AddModelError(string.Empty, "API boş yanıt döndü.");
                return View(new DashboardStats()); // Boş modelle View
            }

            return View(value);
        }
    }
}
