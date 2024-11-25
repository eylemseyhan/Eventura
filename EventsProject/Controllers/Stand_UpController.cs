using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Controllers
{
    public class Stand_UpController : Controller
    {
        Context db = new Context();
        public IActionResult Index()
        {
            var values = db.Events.Where(x => x.CategoryId == 3).ToList();
            return View(values);
        }
        // Detayları Görüntüleme
        public IActionResult Details(int id)
        {
            var eventDetail = db.Events.FirstOrDefault(x => x.EventId == id);
            if (eventDetail == null)
            {
                return NotFound();
            }
            return View(eventDetail);
        }


    }
}