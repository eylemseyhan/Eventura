using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Controllers
{
    public class TheaterController : Controller
    {
        Context db = new Context();  // DbContext'i başlatıyoruz

        public IActionResult Index()
        {
            
            var theaters = db.Events.Where(e => e.CategoryId == 2).ToList();
            return View(theaters);
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
