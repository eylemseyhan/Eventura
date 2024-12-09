using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Areas.Member.Controllers
{
    public class EventController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
