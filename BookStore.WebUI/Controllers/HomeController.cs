using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebUI.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
