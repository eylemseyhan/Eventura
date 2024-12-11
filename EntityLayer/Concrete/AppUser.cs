using Microsoft.AspNetCore.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EntityLayer.Concrete
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Image { get; set; }
        public string? Email { get; set; }
      

        
      
        public virtual ICollection<Ticket> Ticket { get; set; }
        public virtual ICollection<UserFavorite> UserFavorites { get; set; }
        public virtual ICollection<SavedCard> SavedCards { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }


    }
}