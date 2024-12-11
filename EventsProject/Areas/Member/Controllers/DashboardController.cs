using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using EventsProject.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;
namespace EventsProject.Areas.Member.Controllers
{
    [Area("Member")]
    [Authorize] // Sadece giriş yapmış kullanıcılar erişebilir
    public class DashboardController : Controller
    {
        private readonly UserManager<AppUser> _userManager;

        Context db = new Context();

        public DashboardController(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        // Dashboard ana sayfası
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return RedirectToAction("SignIn", "Login", new { area = "Member" });
            }

            var model = new DashboardViewModel
            {
                Name = user.Name,
                Surname = user.Surname,
                Email = user.Email,
                Image = user.Image ?? "/images/default-profile.png" // Varsayılan profil resmi
            };

            return View(model);
        }

        // Profil sayfası
        public IActionResult Profile()
        {
            // Profil sayfası için işlem yapılacak
            return View();
        }

        // Kullanıcı biletleri sayfası
        public IActionResult Tickets()
        {
            // Biletler sayfası için işlem yapılacak
            return View();
        }

        // Kullanıcı favoriler sayfası
        // Kullanıcı favoriler sayfası
        public async Task<IActionResult> Favorites()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("SignIn", "Login", new { area = "Member" });
            }

            var favoriteEvents = await (from uf in db.UserFavorites
                                        join e in db.Events on uf.EventId equals e.EventId
                                        where uf.UserId == user.Id
                                        select new
                                        {
                                            Event = e,
                                            Category = e.Category,
                                            Artist = e.Artist
                                        }).ToListAsync();


            var events = favoriteEvents.Select(uf => uf.Event).ToList();

            return View(events); // Favori etkinlikleri view'a gönderiyoruz
        }

        // Kullanıcı ayarları sayfası
        public IActionResult Settings()
        {
            // Ayarlar sayfası için işlem yapılacak
            return View();
        }
    }
}
