using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Controllers
{
    public class FestivalController : Controller
    {
        Context db = new Context();

        public IActionResult Index()
        {

            var theaters = db.Events.Where(e => e.CategoryId == 5).ToList();
            return View(theaters);
        }
        public IActionResult Details(int id)
        {
            var eventDetail = db.Events.FirstOrDefault(x => x.EventId == id);
            if (eventDetail == null)
            {
                return NotFound(); // Etkinlik bulunamazsa 404 döndür
            }
            return View(eventDetail); // Bu view bir Event nesnesi bekliyor
        }
    }
}
