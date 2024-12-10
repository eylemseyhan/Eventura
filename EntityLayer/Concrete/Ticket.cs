using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class Ticket
    {
        public int TicketId { get; set; }  // Birincil anahtar
        public int EventId { get; set; }    // Etkinlik ID'si
        public int UserId { get; set; }
        public decimal Price { get; set; }  // Bilet fiyatı
        public string TicketNumber { get; set; }
        public bool IsAvailable { get; set; } // Biletin durumu (mevcut / satıldı)
        public int TicketCount { get; set; }


        // İlişkiler
        public virtual AppUser User { get; set; }
        public virtual Event Event { get; set; }
    }
}