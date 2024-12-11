using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class SavedCard
    {
        public int SavedCardId { get; set; }  // Kaydedilmiş kartın benzersiz ID'si
        public int UserId { get; set; }  // Hangi kullanıcıya ait olduğunu belirten ID
        public string CardHolderName { get; set; }  // Kart numarası (şifrelenmiş)
        public string CardNumber { get; set; }  // Kart sahibinin adı
        public DateTime ExpiryDate { get; set; }  // Kartın son kullanma tarihi
        public string CVV { get; set; }  

        public virtual AppUser User { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
