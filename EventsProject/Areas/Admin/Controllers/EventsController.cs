using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Areas.Admin.Controllers
{
    public class EventsController : Controller
    {
        Context db = new Context();
        [Area("Admin")]
        public IActionResult Index()
        {
            var values = db.Events.ToList();
            return View(values);
        }
    }
}