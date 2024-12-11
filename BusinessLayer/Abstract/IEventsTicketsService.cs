using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IEventsTicketsService : IGenericService<EventsTickets>
    {
        
        List<EventsTickets> TGetList(); // Sadece metod imzası
    }
}
