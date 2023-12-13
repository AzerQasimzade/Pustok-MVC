using Microsoft.AspNetCore.Mvc;

namespace PustokBookStore.Controllers
{
    public class ErrorController : Controller
    {

        public IActionResult Index(string error)
        {
            return View(model: error);
        }

    }
}
