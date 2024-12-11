using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EventsProject.Models;
using DataAccessLayer.Concrete; // Ticket ve UserBuy modelleri
using System.Linq;
using System.Security.Claims;

namespace EventsProject.Areas.Member.Controllers
{
    [Area("Member")]
    public class MyTicketController : Controller
    {
        private readonly Context _dbContext;

        // DbContext'i constructor aracılığıyla inject ediyoruz
        public MyTicketController(Context dbContext)
        {
            _dbContext = dbContext;
        }

        // Kullanıcının aldığı biletleri listeleme
        public IActionResult Index()
        {
            // Kullanıcının ID'sini almak için User.Identity kullanıyoruz
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Kullanıcının ID'si

            // Kullanıcının aldığı biletleri ve ilişkili olduğu etkinlik bilgilerini veritabanından çekiyoruz
            var tickets = _dbContext.Tickets
                                     .Include(t => t.Event)  // Etkinlik bilgilerini dahil et
                                     .Include(t => t.EventsTicket)  // Etkinlik bilet bilgilerini dahil et
                                     .Where(t => t.UserId.ToString() == userId) // Kullanıcının ID'siyle eşleşen biletleri alıyoruz
                                     .ToList();

            return View(tickets); // Model olarak tickets verisini view'a gönderiyoruz
        }
    }
}
