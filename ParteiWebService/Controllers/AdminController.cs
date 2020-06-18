using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aufgabe_2.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using DataAccessLibrary.DataAccess;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using Aufgabe_2.Utility;
using System.Text.Encodings.Web;

namespace Aufgabe_2.Controllers
{
    public class AdminController : Controller
    {

        public BobContext _bobContext { get; }
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminController(BobContext bobContext, UserManager<ApplicationUser> userManager)
        {
            _bobContext = bobContext;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View(_bobContext.Organizations.ToList());
        }
        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Manager");
            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> CreateOrganization()
        {
            return View(new Organization());
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrganization(Organization organization)
        {             
            organization.Admin.PasswordHash = Guid.NewGuid().ToString();
            organization.Admin.UserName = organization.Admin.Id;
            _bobContext.Add(organization);
            _bobContext.SaveChanges();
            var response = await _userManager.AddToRoleAsync(organization.Admin, "Manager");

            var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(organization.Admin);
            var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(organization.Admin);

            emailCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailCode));
            passwordCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordCode));


            var fullUrl = this.Url.Action("SetPassword", "Account", new { userId = organization.Admin.Id, emailCode = emailCode, passwordCode = passwordCode }, this.Request.Scheme);

            MailManager mailManager = new MailManager();
            mailManager.SendEmail(
            "Bestätige deine Registrierung",
             $"Bitte bestätige deine Registrierung <a href='{HtmlEncoder.Default.Encode(fullUrl)}'>Hier klicken</a>.",
             organization.Admin.Email,
            organization.Admin.UserName
            );


            return RedirectToAction("Index");
        }

    }
}
