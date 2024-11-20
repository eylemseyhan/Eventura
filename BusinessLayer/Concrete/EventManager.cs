using BusinessLayer.Abstract;
using EntityLayer.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Concrete
{
    public class EventManager : IEventService
    {
        public void TAdd(Event t)
        {
            throw new NotImplementedException();
        }

        public void TDelete(Event t)
        {
            throw new NotImplementedException();
        }

        public Event TGetByID(int id)
        {
            throw new NotImplementedException();
        }

        public List<Event> TGetList()
        {
            throw new NotImplementedException();
        }

        public void TUpdate(Event t)
        {
            throw new NotImplementedException();
        }
    }
}
