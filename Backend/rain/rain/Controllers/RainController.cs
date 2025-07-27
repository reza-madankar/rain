using Microsoft.AspNetCore.Mvc;

namespace rain.Controllers
{
    public class RainController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
