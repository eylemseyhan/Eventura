using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Controllers
{
    public class TheaterController : Controller
    {
        Context db = new Context();  // DbContext'i başlatıyoruz

        public IActionResult Index()
        {
<<<<<<< HEAD
            
=======

>>>>>>> ipek
            var theaters = db.Events.Where(e => e.CategoryId == 2).ToList();
            return View(theaters);
        }

<<<<<<< HEAD
        // Detayları Görüntüleme
=======
>>>>>>> ipek
        public IActionResult Details(int id)
        {
            var eventDetail = db.Events.FirstOrDefault(x => x.EventId == id);
            if (eventDetail == null)
            {
<<<<<<< HEAD
                return NotFound();
            }
            return View(eventDetail);
=======
                return NotFound(); // Etkinlik bulunamazsa 404 döndür
            }
            return View(eventDetail); // Bu view bir Event nesnesi bekliyor
>>>>>>> ipek
        }
    }
}
