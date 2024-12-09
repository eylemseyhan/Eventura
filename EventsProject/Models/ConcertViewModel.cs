using EntityLayer.Concrete;

namespace EventsProject.Models
{
    public class ConcertViewModel
    {
        public List<Event> Events { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
