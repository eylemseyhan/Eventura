using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using EventsProject.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
namespace EventsProject.Areas.Member.Controllers
{
    [Area("Member")]
    [AllowAnonymous]
    public class LoginController : Controller
    {

        private readonly SignInManager<AppUser> _signInManager;

        public LoginController(SignInManager<AppUser> signInManager)
        {
            _signInManager = signInManager;
        }
        [HttpGet]
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(
                    model.Username,
                    model.Password,
                    false,
                    true);
                if (result.Succeeded)
                {
                    // Kullanıcı bilgilerini çek
                    var user = await _signInManager.UserManager.FindByNameAsync(model.Username);

                    if (user != null)
                    {
                        // Kullanıcı bilgilerini claim ekle
                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(identity));

                        // Başarılı giriş sonrası yönlendirme
                        return RedirectToAction("Index", "Home", new { area = "" });
                    }
                }
                else if (result.IsLockedOut)
                {
                    ModelState.AddModelError("", "Hesabınız kilitlenmiştir. Lütfen daha sonra tekrar deneyiniz.");
                }
                else
                {
                    ModelState.AddModelError("", "Geçersiz giriş denemesi. Lütfen bilgilerinizi kontrol ediniz.");
                }
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcı çıkış yapmadan önce Session'ı temizle
            HttpContext.Session.Remove("UserName");

            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Logout sonrası yönlendirme
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}
