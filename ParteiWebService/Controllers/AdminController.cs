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
using Microsoft.AspNetCore.Http;

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
            return View(_bobContext.Organizations.Include(x => x.Admin).ToList());
        }



        [HttpGet]
        public async Task<IActionResult> CreateOrganization()
        {
            return View(new Organization());
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrganization(Organization organization, IFormFile organizationImage)
        {
            var i = organizationImage;
            organization.Admin.PasswordHash = Guid.NewGuid().ToString();
            organization.Admin.UserName = organization.Admin.Id;

            _bobContext.Add(organization);
            _bobContext.SaveChanges();

            organization.Admin.OrgranizationId = organization.Id;
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
