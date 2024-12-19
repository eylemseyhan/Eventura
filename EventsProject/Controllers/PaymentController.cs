using BusinessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace EventsProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly Context _context;

        private readonly IGenericService<SavedCard> _savedCardService; // Generic service
        private readonly IPaymentService _paymentService;

        public PaymentController(Context context, IGenericService<SavedCard> savedCardService,
            IPaymentService paymentService)
        {
            _context = context;
            _savedCardService = savedCardService;
            _paymentService = paymentService;
        }
        [HttpPost]
        public async Task<IActionResult> SaveCard(int eventId, string cardHolderName, string cardNumber,
            string expiryDate, string cvv, bool saveCard, decimal price, int? existingSavedCardId)
        {
            // Bilet kontrolü
            var availableTicket = await _context.Tickets
                .FirstOrDefaultAsync(t => t.IsAvailable && t.UserId == null);

            if (availableTicket == null)
                return BadRequest("Uygun bilet bulunamadı.");

            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Kullanıcı kimliği doğrulanamadı.");
            }

            // SavedCardId'yi başlangıçta existingSavedCardId olarak ata
            int? savedCardId = existingSavedCardId;

            // Eğer yeni kart kaydedilecekse
            if (saveCard && !existingSavedCardId.HasValue)
            {
                if (!ValidateCardDetails(cardNumber, cvv, expiryDate))
                    return BadRequest("Kart bilgileri geçersiz.");

                var savedCard = new SavedCard
                {
                    UserId = userId,
                    CardHolderName = cardHolderName,
                    CardNumber = cardNumber,
                    ExpiryDate = DateTime.ParseExact(expiryDate, new[] { "MM/yy", "MM/yyyy" }, null,
                        System.Globalization.DateTimeStyles.None).ToUniversalTime(),
                    CVV = cvv
                };

                _savedCardService.TAdd(savedCard);
                await _context.SaveChangesAsync();
                savedCardId = savedCard.SavedCardId; // Yeni kaydedilen kartın ID'si
            }

            // Eğer mevcut bir kart kullanılıyorsa ve ID geçerliyse
            if (existingSavedCardId.HasValue)
            {
                var selectedCard = await _context.SavedCards
                    .FirstOrDefaultAsync(c => c.SavedCardId == existingSavedCardId.Value && c.UserId == userId);

                if (selectedCard == null)
                {
                    return BadRequest("Seçilen kayıtlı kart bulunamadı.");
                }

                // Seçilen kartın ID'sini savedCardId'ye ata
                savedCardId = existingSavedCardId.Value;
            }

            // Ödeme kaydı oluştur
            var payment = new Payment
            {
                TicketId = availableTicket.TicketId,
                UserId = userId,
                SavedCardId = savedCardId, // savedCardId doğru şekilde atanıyor
                PaymentStatus = "Başarılı",
                Amount = price,
                PaymentDate = DateTime.UtcNow
            };

            _paymentService.TAdd(payment);
            await _context.SaveChangesAsync();

            try
            {
                bool isTicketPurchased = await _paymentService.BuyTicketAsync(eventId, userId, availableTicket.EventsTicketId);
                if (!isTicketPurchased)
                    return BadRequest("Bilet satın alma işlemi başarısız oldu.");

                return Ok(new { success = true, message = "Ödeme başarıyla tamamlandı." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = $"İşlem sırasında hata oluştu: {ex.Message}" });
            }
        }





        private bool ValidateCardDetails(string cardNumber, string cvv, string expiryDate)
        {
            return long.TryParse(cardNumber.Replace(" ", ""), out _) &&
                   int.TryParse(cvv, out _) &&
                   DateTime.TryParseExact(expiryDate, new[] { "MM/yy", "MM/yyyy" }, null,
                       System.Globalization.DateTimeStyles.None, out _);
        }


        [HttpGet]
        public IActionResult GetSavedCards()
        {
            // Kullanıcı ID'sini Claims'den alıyoruz (Kimlik doğrulama ile bağlantılı)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userIdString))
            {
                return Unauthorized(); // Kullanıcı doğrulaması yapılmamışsa yetkisiz dönebiliriz.
            }

            // SavedCards tablosunda, kullanıcı ID'sine göre kartları filtreliyoruz
            var savedCards = _context.SavedCards
                .Where(c => c.UserId.ToString() == userIdString)
                .Select(c => new
                {
                    c.SavedCardId,
                    CardNumber = "**** **** **** " + c.CardNumber.Substring(c.CardNumber.Length - 4), // Kart numarasını maskelemek için
                    c.CardHolderName,
                    ExpiryDate = c.ExpiryDate.ToString("MM/yy") // Son kullanma tarihini uygun formatta döndürmek için
                })
                .ToList();

            return Ok(savedCards); // Kayıtlı kartların listesini döndür
        }


        [HttpPost]
        public async Task<IActionResult> ProcessPayment(Payment payment)
        {
            if (payment.SavedCardId != null)
            {
                var savedCard = await _context.SavedCards
                    .FirstOrDefaultAsync(card => card.SavedCardId == payment.SavedCardId);

                if (savedCard != null)
                {
                    // Ödeme işlemi mantığı...
                }
            }
            return Json(new { success = false, message = "Kart bulunamadı." });
        }

    }
}