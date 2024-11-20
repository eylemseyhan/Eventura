using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class TicketManager : ITicketService
    {
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
