using BusinessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace EventsProject.Controllers
{
    public class PaymentController : Controller
    {
        private readonly Context _context;

        private readonly IGenericService<SavedCard> _savedCardService; // Generic service
        private readonly IPaymentService _paymentService;

        public PaymentController(Context context, IGenericService<SavedCard> savedCardService, IPaymentService paymentService)
        {
            _context = context;
            _savedCardService = savedCardService;
            _paymentService = paymentService;
        }

        [HttpPost]
        public async Task<IActionResult> SaveCard(int eventId, string cardHolderName,
        string cardNumber, string expiryDate, string cvv, bool saveCard,
        decimal price, int? selectedCardId, int eventTicketId)
        {
            try
            {
                var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
                {
                    return BadRequest("Kullanıcı kimliği doğrulanamadı.");
                }

                int? savedCardId = null;

                // Kayıtlı kart seçilmişse
                if (selectedCardId.HasValue)
                {
                    var existingCard = await _context.SavedCards
                        .FirstOrDefaultAsync(c => c.SavedCardId == selectedCardId.Value && c.UserId == userId);

                    if (existingCard == null)
                    {
                        return BadRequest("Seçilen kayıtlı kart bulunamadı.");
                    }

                    savedCardId = selectedCardId; // Burada direkt selectedCardId'yi atıyoruz
                }
                else
                {
                    // Yeni kart işlemleri...
                    string cleanCardNumber = cardNumber?.Replace(" ", "");

                    if (string.IsNullOrEmpty(cleanCardNumber) || !cleanCardNumber.All(char.IsDigit) ||
                        cleanCardNumber.Length != 16)
                    {
                        return BadRequest("Geçersiz kart numarası");
                    }

                    if (!int.TryParse(cvv, out int cvvParsed) || cvv.Length != 3)
                    {
                        return BadRequest("Geçersiz CVV");
                    }

                    if (!DateTime.TryParseExact(expiryDate, new[] { "MM/yy", "MM/yyyy" },
                        null, System.Globalization.DateTimeStyles.None, out DateTime expiryDateParsed))
                    {
                        return BadRequest("Geçersiz son kullanma tarihi formatı");
                    }

                    if (saveCard)
                    {
                        var savedCard = new SavedCard
                        {
                            UserId = userId,
                            CardHolderName = cardHolderName,
                            CardNumber = cleanCardNumber,
                            ExpiryDate = expiryDateParsed.ToUniversalTime(),
                            CVV = cvv
                        };
                        savedCardId = savedCard.SavedCardId;
                        _savedCardService.TAdd(savedCard);
                        await _context.SaveChangesAsync();

                    }
                }

                // Önce bilet satın alma işlemini gerçekleştir
                bool isTicketPurchased = await _paymentService.BuyTicketAsync(eventId, userId, eventTicketId);

                if (!isTicketPurchased)
                {
                    return BadRequest("Bilet satın alma işlemi başarısız oldu.");
                }

                // Satın alınan bileti bul
                var purchasedTicket = await _context.Tickets
                    .FirstOrDefaultAsync(t => t.EventId == eventId &&
                                            t.EventsTicketId == eventTicketId &&
                                            t.UserId == userId &&
                                            !t.IsAvailable);

                if (purchasedTicket == null)
                {
                    return BadRequest("Satın alınan bilet bulunamadı.");
                }

                // Ödeme kaydı - savedCardId'yi direkt olarak kullanıyoruz
                var payment = new Payment
                {
                    TicketId = purchasedTicket.TicketId,
                    UserId = userId,
                    SavedCardId = savedCardId, // selectedCardId ya da yeni kaydedilen kartın ID'si
                    PaymentStatus = "Başarılı",
                    Amount = price,
                    PaymentDate = DateTime.UtcNow
                };

                _paymentService.TAdd(payment);
                await _context.SaveChangesAsync();

                return Ok("Bilet başarıyla satın alındı.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Bilet satın alınırken bir hata oluştu: {ex.Message}");
            }
        }
        [HttpGet]
        public IActionResult GetSavedCards()
        {
            // Kullanıcı ID'sini Claims'den alıyoruz (Kimlik doğrulama ile bağlantılı)
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier); // Bu, kullanıcının ID'sini alır (örneğin, "123")

            if (userIdString == null)
            {
                return Unauthorized(); // Kullanıcı doğrulaması yapılmamışsa yetkisiz dönebiliriz.
            }

            // SavedCards tablosunda, kullanıcı ID'sine göre kartları filtreliyoruz
            var savedCards = _context.SavedCards
                .Where(c => c.UserId.ToString() == userIdString)
                .Select(c => new
                {
                    c.SavedCardId,
                    c.CardNumber,
                    c.CardHolderName,
                    c.ExpiryDate
                })
                .ToList();

            return Json(savedCards); // JSON olarak dönüyoruz
        }

        [HttpPost]
        public IActionResult ProcessPayment(int savedCardId)
        {
            var savedCard = _context.SavedCards
                                    .FirstOrDefault(card => card.SavedCardId == savedCardId);

            if (savedCard != null)
            {
                // Ödeme işlemi mantığını burada yazın.
                return Json(new { success = true, message = "Ödeme başarılı!" });
            }

            return Json(new { success = false, message = "Kart bulunamadı." });
        }
    }
}