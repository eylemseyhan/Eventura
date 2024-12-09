

using System;
using EntityLayer.Concrete;

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Concrete
{
    public class Context : IdentityDbContext<AppUser, AppRole, int>
    //bu kısımda identity öncesi DbContext den miras alıyordu ama identitiy işlemleri sonrası deiştirerek IdentitityDbContexten miras aldırıdk

    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Server=pg-35d0b49b-eventura.c.aivencloud.com;Port=25558;Database=event;Username=avnadmin;Password=AVNS_UHYggiYYAYpKfGAMhym;Ssl Mode=Require;");
        }

        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<UserFavorite> UserFavorites { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Message> Messages { get; set; }
       

    }
}
