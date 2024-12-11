using EntityLayer.Concrete;

namespace BusinessLayer.Abstract
{
    public interface IEventsTicketsService : IGenericService<EventsTickets>
    {
        // Yeni metodumuzu interface'e ekliyoruz
        List<EventsTickets> GetEventTicketsWithEvents();
    }
}
