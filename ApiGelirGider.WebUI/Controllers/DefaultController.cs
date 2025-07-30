using Microsoft.AspNetCore.Mvc;

namespace ApiGelirGider.WebUI.Controllers
{
    public class DefaultController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
