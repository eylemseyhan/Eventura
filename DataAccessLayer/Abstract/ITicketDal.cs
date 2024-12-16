using EntityLayer.Concrete;
using System.Collections.Generic;

namespace DataAccessLayer.Abstract
{
    public interface ITicketDal : IGenericDal<Ticket>
    {
        void DeleteRange(List<Ticket> tickets);


    }
}
