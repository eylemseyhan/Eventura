using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;

namespace BusinessLayer.Concrete
{
    public class TicketManager : ITicketService
    {
        private readonly ITicketDal _ticketDal;

        public TicketManager(ITicketDal ticketDal)
        {
            _ticketDal = ticketDal;
        }

        public void TAdd(Ticket t)
        {
            // Implementation for adding a single ticket
            _ticketDal.Add(t);
        }

        public void TDelete(Ticket t)
        {
            // Implementation for deleting a ticket
            _ticketDal.Delete(t);
        }

        public Ticket TGetByID(int id)
        {
            // Implementation for getting a ticket by ID
            return _ticketDal.GetByID(id);
        }

        public List<Ticket> TGetList()
        {
            // Implementation for getting a list of tickets
            return _ticketDal.GetList();
        }

        public void TUpdate(Ticket t)
        {
            // Implementation for updating a ticket
            _ticketDal.Update(t);
        }

        // Implement the TAddRange method to add multiple tickets at once
        public void TAddRange(IEnumerable<Ticket> tickets)
        {
            foreach (var ticket in tickets)
            {
                _ticketDal.Add(ticket);
            }
        }

        public void TDeleteRange(List<Ticket> tickets)
        {
            if (tickets == null || !tickets.Any())
            {
                return; // No tickets to delete
            }

            try
            {
                // Use the repository's DeleteRange method
                _ticketDal.DeleteRange(tickets);
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use a logging framework)
                Console.WriteLine($"Error deleting tickets: {ex.Message}");
                throw; // Re-throw to allow calling method to handle
            }
        }
    }
}
