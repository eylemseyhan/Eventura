using BusinessLayer.Abstract;
using DataAccessLayer.Abstract;
using EntityLayer.Concrete;

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
            throw new NotImplementedException();
        }

        public void TDelete(Ticket t)
        {
            throw new NotImplementedException();
        }

        public Ticket TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Ticket> TGetList()
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Ticket t)
        {
            throw new NotImplementedException();
        }
    }
}
