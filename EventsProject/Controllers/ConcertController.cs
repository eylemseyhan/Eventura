using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;


namespace EventsProject.Controllers
{
    public class ConcertController : Controller
    {  private readonly Microsoft.AspNetCore.Identity.UserManager<AppUser> _userManager;

        Context db = new Context();
        public IActionResult Index()
        {
            ViewBag.WelcomeMessage = TempData["WelcomeMessage"];
            var values = db.Events.Where(x => x.CategoryId == 1).ToList();
            return View(values);
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
