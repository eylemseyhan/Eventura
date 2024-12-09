using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Abstract
{
    public interface IEventDal:IGenericDal<Event>
    {

        
        List<Event> TGetListWithArtistAndCategory();
        Event TGetByIDWithArtistAndCategory(int id);
        // Şehre göre etkinlikleri getirme
        List<Event> GetEventsByCity(int cityId);

        // Kategoriye göre etkinlikleri getirme
        List<Event> GetEventsByCategory(int categoryId);

        // Şehir ve kategoriye göre etkinlikleri getirme
        List<Event> GetEventsByCityAndCategory(int cityId, int categoryId);
    }
}
