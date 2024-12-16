using BusinessLayer.Abstract;
using DataAccessLayer.Concrete;
using EntityLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
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



        //        [HttpPost]
        //        public IActionResult SaveCard(int eventId, string cardHolderName, string cardNumber, string expiryDate, string cvv, bool saveCard, decimal price)
        //        {
        //            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        //            // Kart numarası ve CVV doğrulama
        //            if (!long.TryParse(cardNumber.Replace(" ", ""), out long cardNumberParsed))
        //            {
        //                return BadRequest("Geçersiz kart numarası");
        //            }

        //            if (!int.TryParse(cvv, out int cvvParsed))
        //            {
        //                return BadRequest("Geçersiz CVV");
        //            }

        //            if (!DateTime.TryParseExact(expiryDate, new[] { "MM/yy", "MM/yyyy" }, null, System.Globalization.DateTimeStyles.None, out DateTime expiryDateParsed))
        //            {
        //                return BadRequest("Geçersiz son kullanma tarihi formatı");
        //            }

        //            DateTime expiryDateUtc = expiryDateParsed.ToUniversalTime();

        //            // Uygun bilet bulun
        //            var availableTicket = _context.Tickets.FirstOrDefault(t => t.IsAvailable && t.UserId == null);
        //            if (availableTicket == null)
        //            {
        //                return BadRequest("Uygun bilet bulunamadı.");
        //            }

        //            // Fiyatı ViewBag yerine gelen price kullanıyoruz
        //            decimal ticketPrice = ViewBag.TicketPrice;

        //            // Kart kaydetme ve ödeme işlemi
        //            if (saveCard)
        //            {
        //                var savedCard = new SavedCard
        //                {
        //                    UserId = int.Parse(userId),
        //                    CardHolderName = cardHolderName,
        //                    CardNumber = cardNumber,
        //                    ExpiryDate = expiryDateUtc,
        //                    CVV = cvv
        //                };

        //                _savedCardService.TAdd(savedCard);

        //                // Bileti kullanıcıya ata
        //                availableTicket.UserId = int.Parse(userId);
        //                availableTicket.IsAvailable = false;
        //                _context.Tickets.Update(availableTicket);

        //                // Ödeme kaydını oluştur
        //                var payment = new Payment
        //                {
        //                    TicketId = availableTicket.TicketId,
        //                    UserId = int.Parse(userId),
        //                    SavedCardId = savedCard.SavedCardId,
        //                    PaymentStatus = "Başarılı",
        //                    Amount = ticketPrice,  // Burada price kullanılıyor
        //                    PaymentDate = DateTime.UtcNow
        //                };

        //                _paymentService.TAdd(payment);
        //                _context.SaveChanges();
        //            }

        //            return Ok("Ödeme ve kart kaydı başarıyla tamamlandı.");
        //        }


        //    }
        //}

        [HttpPost]
        public async Task<IActionResult> SaveCard(int eventId, string cardHolderName, string cardNumber, string expiryDate, string cvv, bool saveCard, decimal price)
        {
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                return BadRequest("Kullanıcı kimliği doğrulanamadı.");
            }

            // Kart numarası ve CVV doğrulama
            if (!long.TryParse(cardNumber.Replace(" ", ""), out long cardNumberParsed))
            {
                return BadRequest("Geçersiz kart numarası");
            }

            if (!int.TryParse(cvv, out int cvvParsed))
            {
                return BadRequest("Geçersiz CVV");
            }

            if (!DateTime.TryParseExact(expiryDate, new[] { "MM/yy", "MM/yyyy" }, null, System.Globalization.DateTimeStyles.None, out DateTime expiryDateParsed))
            {
                return BadRequest("Geçersiz son kullanma tarihi formatı");
            }

            DateTime expiryDateUtc = expiryDateParsed.ToUniversalTime();

            // Uygun bilet bulun
            var availableTicket = _context.Tickets.FirstOrDefault(t => t.IsAvailable && t.UserId == null);
            if (availableTicket == null)
            {
                return BadRequest("Uygun bilet bulunamadı.");
            }

            decimal ticketPrice = price; // Fiyatı al

            int? savedCardId = null; // Varsayılan olarak null

            // Kartı kaydetme işlemi (checkbox işaretli ise)
            if (saveCard)
            {
                var savedCard = new SavedCard
                {
                    UserId = userId,
                    CardHolderName = cardHolderName,
                    CardNumber = cardNumber,
                    ExpiryDate = expiryDateUtc,
                    CVV = cvv
                };

                _savedCardService.TAdd(savedCard);
                _context.SaveChanges(); // ID üretilir

                savedCardId = savedCard.SavedCardId; // SavedCardId'yi al
            }



            // Ödeme kaydını oluştur
            var payment = new Payment
            {
                TicketId = availableTicket.TicketId,
                UserId = userId,
                SavedCardId = savedCardId, // Eğer kart kaydedilmediyse null olur
                PaymentStatus = "Başarılı",
                Amount = ticketPrice,
                PaymentDate = DateTime.UtcNow
            };

            _paymentService.TAdd(payment);
            _context.SaveChanges();

            try
            {
                bool isTicketPurchased = await _paymentService.BuyTicketAsync(eventId, userId, (int)availableTicket.EventsTicketId);

                if (!isTicketPurchased)
                {
                    return BadRequest("Bilet satın alma işlemi başarısız oldu.");
                }

                return Ok("Bilet başarıyla satın alındı.");
            }
            catch (Exception ex)
            {
                // Detaylı hata mesajını döndür
                return BadRequest($"Bilet satın alınırken bir hata oluştu: {ex.Message}");
            }

            return Ok("Ödeme başarıyla tamamlandı.");
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