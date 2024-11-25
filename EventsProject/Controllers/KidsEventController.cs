using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;

namespace EventsProject.Controllers
{
    public class KidsEventController : Controller
    {
        Context db = new Context();
        public IActionResult Index()
        {
            var values = db.Events.Where(x => x.CategoryId == 4).ToList();
            return View(values);
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
        }
    }
}
=======
                return NotFound(); // Etkinlik bulunamazsa 404 döndür
            }
            return View(eventDetail); // Bu view bir Event nesnesi bekliyor
        }
    }
}
>>>>>>> ipek
