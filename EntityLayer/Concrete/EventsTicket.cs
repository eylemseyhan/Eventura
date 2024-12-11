using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class EventsTicket
    {
        [Key]
        public int EventsTicketId { get; set; }
        public int EventId{ get; set; }
        public int TicketCapacity { get; set; }
        public decimal Price { get; set; }  // Bilet fiyatı
        public string Name { get; set; }
        public int SoldCount { get; set; }


        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }

    }
}
