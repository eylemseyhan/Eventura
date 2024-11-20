using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EntityLayer.Concrete
{
    public class City
    {
        public int CityId { get; set; }      // Şehir ID'si
        public string Name { get; set; }     // Şehir adı (örn: İstanbul, Ankara, vb.)

        // Şehre ait etkinliklerin listesi
        public virtual ICollection<Event> Events { get; set; }
    }
}
