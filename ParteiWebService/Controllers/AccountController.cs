using System.Text;
using System.Threading.Tasks;
using Aufgabe_2.Models;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;

namespace Aufgabe_2.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BobContext _bobContext;

        public AccountController(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BobContext bobContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bobContext = bobContext;
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> LoginAsync(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                var signInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInResult.Succeeded)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("Index", "Admin", new { area = "" });
                    }
                    return RedirectToAction("Index", "Home", new { area = "" });
                }
            }
            return RedirectToAction("Login");
        }

        [HttpPost]
        public async Task<IActionResult> RegisterAsync(string username, string password)
        {
            var user = new ApplicationUser
            {
                UserName = username,
                Email = ""
            };

            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return RedirectToAction("Login");
            }
            return RedirectToAction("Register");
        }

        public async Task<IActionResult> SetPassword(string userId, string emailCode, string passwordCode)
        {
            if (userId == null || emailCode == null || passwordCode == null)
            {
                return RedirectToPage("Home", "Index");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }


            return View(new SetPasswordModel() { UserId = user.Id, PasswordCode = passwordCode, EmailConfirmationCode = emailCode });
        }

        [HttpPost]
        public async Task<IActionResult> SetPassword(SetPasswordModel setPasswordModel)
        {
            if (setPasswordModel.UserId == null || setPasswordModel.EmailConfirmationCode == null || setPasswordModel.PasswordCode == null)
            {
                return RedirectToPage("Home", "Index");
            }

            var user = await _userManager.FindByIdAsync(setPasswordModel.UserId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{setPasswordModel.UserId}'.");
            }

            setPasswordModel.PasswordCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(setPasswordModel.PasswordCode));
            setPasswordModel.EmailConfirmationCode = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(setPasswordModel.EmailConfirmationCode));
            var emailResult = await _userManager.ConfirmEmailAsync(user, setPasswordModel.EmailConfirmationCode);
            var resetPasswordResult = await _userManager.ResetPasswordAsync(user, setPasswordModel.PasswordCode, setPasswordModel.Password);
            await _userManager.SetUserNameAsync(user, setPasswordModel.Username);

            return RedirectToAction("Login");
        }


        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}