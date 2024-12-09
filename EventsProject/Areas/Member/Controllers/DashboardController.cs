using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Areas.Member.Controllers
{
    [Area("Member")]
    public class DashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
