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
            public decimal Price { get; set; }  // Bilet fiyatı
            public string SeatNumber { get; set; } // Koltuk numarası
            public bool IsAvailable { get; set; } // Biletin durumu (mevcut / satıldı)

            // İlişkiler
            public virtual Event Event { get; set; }
        }
}


