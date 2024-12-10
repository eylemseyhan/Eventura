using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        // Bilet Alma İşlemi
        [HttpPost]
        public IActionResult BuyTicket(int eventId)
        {
            if (User.Identity.IsAuthenticated)
            {
                // Kullanıcı Giriş Yaptıysa TicketCount'u Azalt
                var eventDetail = db.Tickets.FirstOrDefault(x => x.EventId == eventId);
                if (eventDetail != null && eventDetail.TicketCount > 0)
                {
                    eventDetail.TicketCount--;  // TicketCount 1 azalır
                    db.SaveChanges();  // Veritabanı değişikliklerini kaydet
                    return RedirectToAction("Details", new { id = eventId });
                }
                else
                {
                    TempData["Message"] = "Biletler tükenmiş veya etkinlik bulunamadı!";
                    return RedirectToAction("Details", new { id = eventId });
                }
            }
            else
            {
                TempData["Message"] = "Lütfen giriş yapınız!";
                return RedirectToAction("Login", "Account");
            }
        }


    }
}
