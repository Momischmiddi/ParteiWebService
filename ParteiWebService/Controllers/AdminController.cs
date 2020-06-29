using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ParteiWebService.Models;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using DataAccessLibrary.DataAccess;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using ParteiWebService.Utility;
using System.Text.Encodings.Web;

namespace ParteiWebService.Controllers
{
    public class AdminController : Controller
    {

        public ParteiDbContext _parteiDbContext { get; }
        private readonly UserManager<ApplicationUser> _userManager;


        public AdminController(ParteiDbContext parteiDbContext, UserManager<ApplicationUser> userManager)
        {
            _parteiDbContext = parteiDbContext;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            return View(_parteiDbContext.Organizations.ToList());
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
            _parteiDbContext.Add(organization);
            _parteiDbContext.SaveChanges();
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
