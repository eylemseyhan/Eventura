using EntityLayer.Concrete;
using System.Collections.Generic;

namespace BusinessLayer.Abstract
{
    public interface IEventService
    {
        // Etkinlikleri şehre göre getirme
        List<Event> GetEventsByCity(int cityId);

        // Etkinlikleri kategoriye göre getirme
        List<Event> GetEventsByCategory(int categoryId);

        // Etkinlikleri hem şehre hem de kategoriye göre getirme
        List<Event> GetEventsByCityAndCategory(int cityId, int categoryId);

        List<Event> TGetList();
        List<Event> GetAllEvents();
      
    }
}
