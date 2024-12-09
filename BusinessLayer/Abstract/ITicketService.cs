using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface ITicketService : IGenericService<Ticket>
    {
        List<Ticket> GetTicketsWithEvents(); // Event ile birlikte Ticket listesi



    }
}
