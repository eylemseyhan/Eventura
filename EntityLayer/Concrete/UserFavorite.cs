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
    public class UserFavorite
    {
        [Key] // Birincil anahtar
        public int UserFavoriteId { get; set; }

     
        public int UserId { get; set; }

      
        public int EventId { get; set; }

        // İlişkiler
        public virtual AppUser User { get; set; } // AppUser ile bire çok ilişki
        public virtual Event Event { get; set; } // Event ile bire çok ilişki
    }

}
