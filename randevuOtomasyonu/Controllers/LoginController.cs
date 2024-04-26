using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using randevuOtomasyonu.Models;


public class LoginController : Controller
{
    private readonly RandevuprojeContext _context;

    public LoginController(RandevuprojeContext context)
    {
        _context = context;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string sifre, string returnUrl)
    {
        var user = await _context.Logins.FirstOrDefaultAsync(u => u.Email == email && u.Sifre == sifre);

        if (user != null)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),

                new Claim(ClaimTypes.Role, "User"),
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        else
        {
            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre");
            return View();
        }
    }

    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Login", "Login");
    }
}