using DataAccessLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace EventsProject.ViewComponents
{
    public class _UILayoutUpcomingEventsComponent : ViewComponent
    {
        private readonly Context db = new Context();

        public IViewComponentResult Invoke(int page = 1)
        {
            // Her sayfada gösterilecek etkinlik sayısı
            int eventsPerPage = 4;

            // DateTimeOffset.Now kullanarak zaman dilimi ile geçerli tarih ve saat alınır
            var currentTime = DateTimeOffset.Now;

            // UTC'ye dönüştürme (zaman dilimi bilgisiyle)
            var utcCurrentTime = currentTime.ToUniversalTime(); // UTC'ye dönüştür

            // Etkinlikleri tarihine göre sıralayıp, her sayfada 4 etkinlik gösterecek şekilde verileri alıyoruz
            var upcomingEvents = db.Events
                .Where(e => e.EventDate > utcCurrentTime) // Tarih karşılaştırmasını UTC ile yap
                .OrderBy(e => e.EventDate) // Tarihe göre sıralama
                .Skip((page - 1) * eventsPerPage) // Sayfa numarasına göre atlama
                .Take(eventsPerPage) // Sayfada gösterilecek etkinlik sayısı
                .ToList(); // Tam Event nesnesini al

            return View(upcomingEvents); // Modeli tam olarak gönder
        }

    }
}
