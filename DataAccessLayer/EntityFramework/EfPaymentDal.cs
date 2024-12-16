using DataAccessLayer.Abstract;
using DataAccessLayer.Concrete;
using DataAccessLayer.Repository;
using EntityLayer.Concrete;
using Microsoft.EntityFrameworkCore; // EF Core kullanıyoruz
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLayer.EntityFramework
{
    public class EfPaymentDal : GenericRepository<Payment>, IPaymentDal
    {
        public async Task<bool> BuyTicketAsync(int eventId, int userId, int eventTicketId)
        {
            try
            {
                using (var context = new Context())
                {
                    // Transaction başlatıyoruz
                    using (var transaction = await context.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // Uygun bir bilet bulmaya çalışıyoruz
                            var ticket = await context.Tickets
                                .Where(t => t.EventId == eventId && t.UserId == null && t.IsAvailable)
                                .OrderBy(t => t.TicketId) // En uygun bileti almak için ID'ye göre sıralayabilirsiniz
                                .FirstOrDefaultAsync();

                            if (ticket == null)
                            {
                                throw new Exception("Uygun bilet bulunamadı.");
                            }

                            var eventTicket = await context.EventsTickets
                                .FirstOrDefaultAsync(et => et.EventsTicketId == eventTicketId);

                            if (eventTicket != null)
                            {
                                eventTicket.SoldCount++;
                                context.EventsTickets.Update(eventTicket); // Değişikliği açıkça bildir
                            }

                            // Biletin durumunu güncelliyoruz
                            ticket.UserId = userId;
                            ticket.IsAvailable = false;
                            await context.SaveChangesAsync();

                            await transaction.CommitAsync();

                            return true;
                        }
                        catch (Exception ex)
                        {
                            await transaction.RollbackAsync();
                            throw new Exception($"Bilet satın alma işlemi sırasında bir hata oluştu: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"BuyTicketAsync genel hata: {ex.Message}");
            }
        }


        public decimal GetEventTicketPrice(int id)
        {
            using (var context = new Context())
            {
                // EventId ile EventTickets tablosundan fiyatı alıyoruz
                var price = context.EventsTickets
                    .Where(et => et.EventId == id)
                    .Select(et => et.Price)
                    .FirstOrDefault();

                return price;
            }
        }
    }
}