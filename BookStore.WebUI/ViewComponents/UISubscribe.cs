using Microsoft.AspNetCore.Mvc;

namespace BookStore.WebUI.ViewComponents
{
    public class UISubscribe : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
