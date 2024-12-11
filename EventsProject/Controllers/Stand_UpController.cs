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
 [HttpPost]
public IActionResult BuyTicket(int eventId)
{
    if (!User.Identity.IsAuthenticated)
    {
        TempData["Message"] = "Lütfen giriş yapınız!";
        return RedirectToAction("SignIn", "Login", new { area = "Member" });
    }

    var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

    var ticket = db.Tickets
        .Where(t => t.EventId == eventId && t.UserId == null && t.IsAvailable)
        .FirstOrDefault();

    if (ticket == null)
    {
        TempData["Message"] = "Biletler tükenmiş veya etkinlik bulunamadı!";
        return RedirectToAction("Details", new { id = eventId });
    }

            // Etkinlik için soldout ve capacity değerlerini kontrol et
            var eventTicket = db.EventsTickets
                .Where(et => et.EventId == eventId && et.EventsTicketId == ticket.EventsTicketId)
                .FirstOrDefault();

            if (eventTicket != null && eventTicket.SoldCount == eventTicket.TicketCapacity)
            {
                // Etkinlik kapasitesi dolmuşsa pop-up mesajı göster
                TempData["ShowSoldOutPopup"] = true;
                return RedirectToAction("Details", new { id = eventId });
            }

            ticket.UserId = userId;
    ticket.IsAvailable = false;

    if (eventTicket != null)
    {
        eventTicket.SoldCount += 1;
    }

    db.SaveChanges();

    TempData["Message"] = "Bilet başarıyla alındı!";
    return RedirectToAction("Details", new { id = eventId });
}


    }
}
