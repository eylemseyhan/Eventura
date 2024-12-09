using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using DataAccessLayer.Concrete;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Concrete
{
    public class TicketManager : ITicketService
    {
        private readonly ITicketDal _ticketDal;

        public TicketManager(ITicketDal ticketDal)
        {
            _ticketDal = ticketDal;
        }


        // Rastgele bilet numarası oluşturma
        private string GenerateTicketNumber()
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var buffer = new byte[8];
            rng.GetBytes(buffer);
            return Convert.ToBase64String(buffer)
                .Substring(0, 8)
                .ToUpperInvariant();
        }



        // Bilet ekleme
        public void TAdd(Ticket ticket)
        {
            if (ticket.TicketCount <= 0)
            {
                throw new ArgumentException("Ticket count must be greater than zero.");
            }

            if (ticket.EventId == 0)
            {
                throw new ArgumentException("EventId must be valid.");
            }

            for (int i = 0; i < ticket.TicketCount; i++)
            {
                var newTicket = new Ticket
                {
                    EventId = ticket.EventId,
                    Price = ticket.Price,
                    IsAvailable = ticket.IsAvailable,
                    TicketNumber = GenerateTicketNumber(), // Ensure this is correctly set
                };

                try
                {
                    _ticketDal.Insert(newTicket);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Bilet eklenirken hata oluştu: {ex.Message}");
                    throw;
                }
            }
        }




        // Bilet güncelleme
        public void TUpdate(Ticket ticket)
        {
            _ticketDal.Update(ticket); // Kategoriyi güncelleme
        }






        // Bilet silme
        public void TDelete(Ticket ticket)
        {
            _ticketDal.Delete(ticket);
        }

        // ID'ye göre bilet getirme
        public Ticket TGetByID(int id)
        {
            return _ticketDal.GetByID(id);
        }

        // Tüm biletleri listeleme
        public List<Ticket> TGetList()
        {
            return _ticketDal.GetList();
        }

        // Etkinliklerle birlikte biletleri listeleme
        public List<Ticket> GetTicketsWithEvents()
        {
            using (var context = new Context())
            {
                return context.Tickets
                    .Include(t => t.Event)
                    .ThenInclude(e => e.Artist)  // Example of loading Artist
                    .ToList();
            }
        }




    }
}