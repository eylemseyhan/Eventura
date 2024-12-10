using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
