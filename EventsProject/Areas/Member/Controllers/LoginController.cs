using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EntityLayer.Concrete;
using EventsProject.Areas.Member.Models;
using Microsoft.AspNetCore.Authorization;
using EventsProject.Areas.Member.Models;
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
                  false, true);

                if (result.Succeeded)
                {
                    // Başarılı girişten sonra yönlendirme yapılır
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
            return View(model);
        }


        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn", "Login");
        }

    }
}