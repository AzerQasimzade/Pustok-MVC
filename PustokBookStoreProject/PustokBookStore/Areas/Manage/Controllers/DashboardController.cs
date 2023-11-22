using Microsoft.AspNetCore.Mvc;

namespace PustokBookStore.Areas.PustokAdmin.Controllers
{
    public class DashboardController : Controller
    {
        [Area("Manage")]
        
            public IActionResult Index()
            {
                return View();
            }        
    }
}
