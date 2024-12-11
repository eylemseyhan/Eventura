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


        //[HttpPost]
        //public IActionResult BuyTicket(int eventId)
        //{
        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        TempData["Message"] = "Lütfen giriş yapınız!";
        //        return RedirectToAction("SignIn", "Login", new { area = "Member" });
        //    }

        //    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        //    var ticket = db.Tickets.FirstOrDefault(x => x.EventId == eventId && x.IsAvailable && x.TicketCount > 0);

        //    if (ticket == null)
        //    {
        //        TempData["Message"] = "Biletler tükenmiş veya etkinlik bulunamadı!";
        //        return RedirectToAction("Details", new { id = eventId });
        //    }

        //    if (db.Tickets.Any(x => x.EventId == eventId && x.UserId == userId))
        //    {
        //        TempData["Message"] = "Bu etkinlik için zaten bilet aldınız.";
        //        return RedirectToAction("Details", new { id = eventId });
        //    }

        //    ticket.TicketCount--;
        //    ticket.UserId = userId;
        //    ticket.IsAvailable = ticket.TicketCount > 0;

        //    db.SaveChanges();

        //    TempData["Message"] = "Bilet başarıyla alındı!";
        //    return RedirectToAction("Details", new { id = eventId });
        //}

        [HttpPost]
        public IActionResult BuyTicket(int eventId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["Message"] = "Lütfen giriş yapınız!";
                return RedirectToAction("SignIn", "Login", new { area = "Member" });
            }

            // Giriş yapan kullanıcının ID'sini alıyoruz
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var ticket = db.Tickets
                  .Where(t => t.EventId == eventId && t.UserId == null && t.IsAvailable)
                  .FirstOrDefault();

            if (ticket == null)
            {
                // Eğer bilet bulunamazsa ya da tükenmişse kullanıcıya mesaj veriyoruz
                TempData["Message"] = "Biletler tükenmiş veya etkinlik bulunamadı!";
                return RedirectToAction("Details", new { id = eventId });
            }

            // Bilet alınmış oldu, UserId ve IsAvailable alanlarını güncelliyoruz
            ticket.UserId = userId;   // Kullanıcı ID'si ekleniyor
            ticket.IsAvailable = false;  // Bilet alınmış, artık mevcut değil

            // Değişiklikleri kaydediyoruz
            db.SaveChanges();

            // Kullanıcıya başarı mesajı gösteriyoruz
            TempData["Message"] = "Bilet başarıyla alındı!";
            return RedirectToAction("Details", new { id = eventId });
        }


    }
}
