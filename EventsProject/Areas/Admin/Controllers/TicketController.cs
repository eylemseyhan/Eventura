using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Collections.Generic;

namespace EventsProject.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TicketController : Controller
    {
        private readonly ITicketService _ticketService;
        private readonly IEventsTicketsService _eventsTicketService;
        private readonly IEventService _eventService;
        private readonly ILogger<TicketController> _logger;

        public TicketController(IEventsTicketsService eventsTicketService, IEventService eventService, ILogger<TicketController> logger, ITicketService ticketService)
        {
            _eventsTicketService = eventsTicketService;
            _ticketService = ticketService;
            _eventService = eventService;
            _logger = logger;
        }

        // Event biletlerini listeleme
        public IActionResult Index()
        {
            var tickets = _eventsTicketService.TGetList() ?? new List<EventsTickets>();

            // GetEventNames metodunu kullanarak etkinlik başlıklarını alıyoruz
            var eventNames = _eventsTicketService.GetEventNames();

            ViewBag.EventNames = eventNames; // Etkinlik başlıklarını ViewBag ile gönderiyoruz
            return View(tickets);
        }


        // GET: Admin/Ticket/Create
        public ActionResult Create()
        {
            ViewBag.Events = _eventService.GetAllEvents();  // Event listesini ViewBag ile gönderiyoruz
            return View();  // Create.cshtml'ye yönlendiriyor
        }

        // POST: Admin/Ticket/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(EventsTickets eventsTicket)
        {
            if (ModelState.IsValid)
            {
                // Etkinlik bileti ekle
                _eventsTicketService.TAdd(eventsTicket);
                for (int i = 0; i < eventsTicket.TicketCapacity; i++)
                {
                    var ticketNumber = GenerateTicketNumber(eventsTicket.EventId, i);
                    var ticket = new Ticket
                    {
                        EventsTicketId = eventsTicket.EventsTicketId,
                        TicketNumber = ticketNumber,
                        IsAvailable = true
                    };
                    _ticketService.TAdd(ticket);
                }
                return RedirectToAction("Index");  // Başarıyla ekledikten sonra listeye dön
            }

            ViewBag.Events = _eventService.GetAllEvents(); // Hata durumunda etkinlik listesi tekrar gönderilir
            return View(eventsTicket);  // Formu tekrar gösteriyoruz
        }

        private string GenerateTicketNumber(int eventId, int index)
        {
            // Ticket numarasını belirli bir formatta oluşturabiliriz.
            return $"{eventId}-{index + 1:D4}"; // Örneğin: EventId-0001, EventId-0002, vb.
        }
    

    // Bilet düzenleme sayfası
    public IActionResult Edit(int id)
        {
            var ticket = _eventsTicketService.TGetByID(id);
            if (ticket == null)
            {
                return NotFound(); // Bilet bulunmazsa hata sayfası göster
            }

            ViewBag.Events = _eventService.TGetList(); // Etkinlikleri ViewBag'de getiriyoruz
            return View(ticket);
        }

        // Bilet düzenleme işlemi
        [HttpPost]
        public IActionResult Edit(EventsTickets ticket)
        {
            if (ModelState.IsValid)
            {
                _eventsTicketService.TUpdate(ticket);
                return RedirectToAction("Index");
            }

            ViewBag.Events = _eventService.TGetList(); // Etkinlikler yeniden geliyor
            return View(ticket);
        }

        // Bilet silme işlemi
        [HttpPost]
        public IActionResult Delete(int id)
        {
            var ticket = _eventsTicketService.TGetByID(id);
            if (ticket != null)
            {
                _eventsTicketService.TDelete(ticket);
                return RedirectToAction("Index");
            }
            return NotFound(); // Bilet bulunmazsa hata sayfası göster
        }
    }
}
