using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Collections.Generic;


namespace BusinessLayer.Abstract
{
    public interface IEventService
    {
        // Etkinlikleri şehre göre getirme
        List<Event> GetEventsByCity(int cityId);
        public Event TGetByID(int id)
        {
            using var context = new Context();
            return context.Events.FirstOrDefault(e => e.EventId == id);
        }


        // Etkinlikleri kategoriye göre getirme
        List<Event> GetEventsByCategory(int categoryId);

        // Etkinlikleri hem şehre hem de kategoriye göre getirme
        List<Event> GetEventsByCityAndCategory(int cityId, int categoryId);

        List<Event> TGetList();
        List<Event> GetAllEvents();
        Dictionary<string, int> GetEventCountsByMonth();


    }
}
