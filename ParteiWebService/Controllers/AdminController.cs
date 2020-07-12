using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;
using Microsoft.AspNetCore.Authorization;
using DataAccessLibrary.DataAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using ParteiWebService.Utility;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using System.Collections.Generic;

namespace ParteiWebService.Controllers
{
    public class AdminController : Controller
    {
        public ParteiDbContext _parteiDbContext { get; }
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(ParteiDbContext parteiDbContext, UserManager<ApplicationUser> userManager)
        {
            _parteiDbContext = parteiDbContext;;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            // COSMOS BEGIN
            var organizations = CosmosManager.Organizations.Find(new BsonDocument()).ToList();
            return View(CosmosMapper.MapOrganizations(organizations, _parteiDbContext));
            // COSMOS END

            // SQLITE BEGIN
            // var result =  _parteiDbContext.Organizations.Include(x => x.Admin).ToList();
            // return View(result);
            // SQLITE END
        }

        [HttpGet]
        public IActionResult CreateOrganization()
        {
            return View(new Organization());
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrganization(Organization organization, IFormFile organizationImage)
        {
            var i = organizationImage;
            organization.Admin.PasswordHash = Guid.NewGuid().ToString();
            organization.Admin.UserName = organization.Admin.Id;
            // COSMOS BEGIN 

            int id = -1;
            foreach(var org in CosmosManager.Organizations.Find(new BsonDocument()).ToList())
            {
                if(org.Id > id)
                {
                    id = org.Id;
                }
            }
            
            CosmosManager.Organizations.InsertOne(new CosmosDB.DBModels.Organization
            {
                AdminId = organization.Admin.Id,
                Name = organization.Name,
                OrganizationImageUrl = organization.OrganizationImage,
                Id = id +1
            });
            // COSMOS END
            
            _parteiDbContext.Add(organization);
            _parteiDbContext.SaveChanges();
             
            organization.Admin.OrgranizationId = organization.Id;
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
