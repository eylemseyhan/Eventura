using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using DataAccessLayer.Concrete;

namespace BusinessLayer.Concrete
{
    public class EventsTicketManager : IEventsTicketsService
    {
        private readonly IEventsTicketDal _eventsTicketDal;

        public EventsTicketManager(IEventsTicketDal eventsTicketDal)
        {
            _eventsTicketDal = eventsTicketDal;
        }

        public void TAdd(EventsTickets t)
        {
            _eventsTicketDal.Insert(t); // Insert işlemi
        }

        public void TDelete(EventsTickets t)
        {
            _eventsTicketDal.Delete(t); // Delete işlemi
        }

        public EventsTickets TGetByID(int id)
        {
            return _eventsTicketDal.GetByID(id); // ID'ye göre bilet getirme
        }

        public List<EventsTickets> TGetList()
        {
            return _eventsTicketDal.GetList(); // Bütün biletleri listeleme
        }

        public void TUpdate(EventsTickets t)
        {
            _eventsTicketDal.Update(t); // Güncelleme işlemi
        }

        public List<EventsTickets> GetEventTicketsWithEvents()
        {
            using (var context = new Context())
            {
                return context.EventsTickets
                    .Include(et => et.Events)      // Event ilişkisini dahil ediyoruz
                    .ThenInclude(e => e.Artist)    // Event'in Artist ilişkisini dahil ediyoruz
                    .ToList();
            }
        }

    }
}
