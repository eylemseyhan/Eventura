using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EventsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IEventService _eventService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(ITicketService ticketService, IEventService eventService, ILogger<TicketController> logger)
        {
            _ticketService = ticketService;
            _eventService = eventService;
            _logger = logger;
        }

        public ActionResult Create()
        {

            ViewBag.Events = _eventService.GetAllEvents();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Ticket ticket)
        {
            try
            {
                if (ticket.TicketCount <= 0)
                {
                    TempData["ErrorMessage"] = "Bilet adedi en az 1 olmalıdır.";
                    ViewBag.Events = _eventService.GetAllEvents();
                    return View(ticket);
                }

                _ticketService.TAdd(ticket);

                TempData["SuccessMessage"] = "Bilet başarıyla oluşturuldu.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet oluşturma hatası: {ex.Message}");
                TempData["ErrorMessage"] = "Bir hata oluştu.";
            }

            ViewBag.Events = _eventService.GetAllEvents();
            return View(ticket);
        }





        public IActionResult Index()
        {
            var tickets = _ticketService.GetTicketsWithEvents();
            return View(tickets);
        }




        // GET: Admin/Ticket/Edit/5
        [HttpGet]
        // Bilet güncelle (GET)
        // Bilet güncelle (GET)
        // Bilet güncelle (GET)
        // Bilet güncelle (GET)
        [HttpGet]
        public IActionResult Edit(int id)
        {
            try
            {
                var ticket = _ticketService.TGetByID(id);
                if (ticket == null)
                {
                    _logger.LogWarning($"Bilet bulunamadı: {id}");
                    return NotFound();
                }

                // Etkinlik biletlerini almak
                var eventTickets = _ticketService.GetTicketsWithEvents();
                var totalTickets = eventTickets.Count(t => t.EventId == ticket.EventId);

                // TicketCount ve Price'ı güncelle
                ticket.TicketCount = totalTickets;
                ticket.Price = ticket.Price > 0 ? ticket.Price : 0;

                // Etkinlik verilerini ViewBag ile gönder
                ViewBag.Events = _eventService.GetAllEvents();
                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet düzenleme hatası: {ex.Message}");
                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Ticket ticket)
        {
            try
            {
                if (ticket.Price <= 0)
                {
                    TempData["ErrorMessage"] = "Fiyat geçerli olmalıdır.";
                    ViewBag.Events = _eventService.GetAllEvents();
                    return View(ticket);
                }

                // Etkinlik ID'sine göre tüm biletleri al
                var eventTickets = _ticketService.GetTicketsWithEvents().Where(t => t.EventId == ticket.EventId).ToList();

                if (eventTickets == null || !eventTickets.Any())
                {
                    TempData["ErrorMessage"] = "Bu etkinliğe ait biletler bulunamadı.";
                    return RedirectToAction("Index");
                }

                // Etkinlikteki tüm biletlerin fiyatını güncelle
                foreach (var eventTicket in eventTickets)
                {
                    eventTicket.Price = ticket.Price;  // Yeni fiyatı ata
                    eventTicket.IsAvailable = ticket.IsAvailable; // Durumu güncelle
                    _ticketService.TUpdate(eventTicket); // Bilet güncelle
                }

                TempData["SuccessMessage"] = "Tüm biletlerin fiyatı ve durumu başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet düzenleme hatası: {ex.Message} | Stack Trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                ViewBag.Events = _eventService.GetAllEvents();
                return View(ticket);
            }
        }



        // GET: Admin/Ticket/EditTicket/5
        [HttpGet]
        public IActionResult EditTicket(int ticketId)
        {
            try
            {
                var ticket = _ticketService.TGetByID(ticketId);
                if (ticket == null)
                {
                    _logger.LogWarning($"Bilet bulunamadı: {ticketId}");
                    return NotFound();
                }

                // Etkinlik verilerini ViewBag ile gönder
                ViewBag.Events = _eventService.GetAllEvents();
                return View(ticket);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet düzenleme hatası: {ex.Message}");
                return View("Error");
            }
        }

        // POST: Admin/Ticket/EditTicket
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditTicket(Ticket ticket)
        {
            try
            {
                if (ticket.Price <= 0)
                {
                    TempData["ErrorMessage"] = "Fiyat geçerli olmalıdır.";
                    ViewBag.Events = _eventService.GetAllEvents();
                    return View(ticket);
                }

                // Güncellenen biletin fiyat ve durumu
                var existingTicket = _ticketService.TGetByID(ticket.TicketId);
                if (existingTicket == null)
                {
                    TempData["ErrorMessage"] = "Bilet bulunamadı.";
                    return RedirectToAction("Index");
                }

                existingTicket.Price = ticket.Price; // Fiyatı güncelle
                existingTicket.IsAvailable = ticket.IsAvailable; // Durumu güncelle

                // Bilet güncelle
                _ticketService.TUpdate(existingTicket);

                TempData["SuccessMessage"] = "Bilet başarıyla güncellendi.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet düzenleme hatası: {ex.Message} | Stack Trace: {ex.StackTrace}");
                TempData["ErrorMessage"] = $"Bir hata oluştu: {ex.Message}";
                ViewBag.Events = _eventService.GetAllEvents();
                return View(ticket);
            }
        }


        public IActionResult Delete(int id)
        {
            try
            {
                var ticket = _ticketService.TGetByID(id);
                if (ticket == null)
                {
                    _logger.LogWarning($"Bilet silinemedi, bilet bulunamadı: {id}");
                    return NotFound();
                }

                _ticketService.TDelete(ticket); // 
                return RedirectToAction("Index"); // Liste sayfasına yönlendir
            }
            catch (Exception ex)
            {
                _logger.LogError($"Bilet silme hatası: {ex.Message}"); // Hata mesajını log'a yazdırıyoruz
                return View("Error");
            }
        }







    }
}