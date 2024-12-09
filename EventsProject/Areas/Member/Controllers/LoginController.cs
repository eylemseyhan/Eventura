using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using EventsProject.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;

namespace EventsProject.Areas.Member.Controllers
{
    [Area("Member")]
    [AllowAnonymous]
    public class LoginController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public LoginController(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        // Giriş sayfası
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
                    false, true);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home", new { area = "" });
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
            return View(model); // Burada LoginViewModel modeli geri döndürülmeli
        }


        // Çıkış işlemi
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // Kullanıcıyı çıkartıyoruz
            await _signInManager.SignOutAsync();

            // Kullanıcıyı "Index" sayfasına yönlendiriyoruz
            // Areas dışındaki "Home" controller ve "Index" action
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}