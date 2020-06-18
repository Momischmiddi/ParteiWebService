using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Aufgabe_2.CSV_Export;
using Aufgabe_2.ExportManagers;
using Aufgabe_2.MicroServiceHelpers.PDFService;
using Aufgabe_2.Models;
using Aufgabe_2.Utility;
using Aufgabe_2.Views.Manager;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

namespace Aufgabe_2.Controllers
{
    public class HomeController : Controller
    {
        private readonly BobContext _bobContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(BobContext bobContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _bobContext = bobContext;
            _roleManager = roleManager;
            _userManager = userManager;
            createRolesandUsers();
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            /*
            PDFExportManager.CreateMemberPDF(new ModelMember
            {
                Users = new List<User>
                {
                    new User
                    {
                        PreName = "Moritz",
                        LastName = "Schmidt",
                        Address = "Domänenstraße 16",
                        Age = 24,
                        City = "Tettnang",
                        Contribution = 12.01,
                        Postal = "88069"
                    },

                    new User
                    {
                        PreName = "Moritz",
                        LastName = "Schmidt",
                        Address = "Domänenstraße 16",
                        Age = 24,
                        City = "Tettnang",
                        Contribution = 12.13,
                        Postal = "88069"
                    }
                }
            });
            */
            var user = await _userManager.GetUserAsync(User);
            var organizsationUser = _bobContext.Organizations.Include(i => i.Admin).Where(u => u.Admin.Id.Equals(user.Id)).SingleOrDefault();
            if(organizsationUser != null)
            {
                var allMembers = _bobContext.Members.Where(a => a.Organization.Id.Equals(organizsationUser.Id)).ToList();
                return View(allMembers);
            }
            else
            {
                return RedirectToAction("Account", "Login");
            }
            
        }

        [HttpPost]
        public async Task<IActionResult> AddMemberAsync(Member member)
        {
            member.ID = Guid.NewGuid().ToString();
            var user = await _userManager.GetUserAsync(User);
            var organizsationUser = _bobContext.Organizations.Where(u => u.Admin.Id.Equals(user.Id)).SingleOrDefault();
            member.OrganizationId = organizsationUser.Id;
            if (!member.IsActiveMember)
            {
                member.ApplicationUser = null;
                member.ApplicationUserId = null;
            }
            _bobContext.Add(member);
            _bobContext.SaveChanges();

            if(member.IsActiveMember)
            {
               
                if (member.ApplicationUser == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                member.ApplicationUser.UserName = member.ID;
                member.ApplicationUser.PasswordHash = Guid.NewGuid().ToString();
                _bobContext.SaveChanges();

                var userId = await _userManager.GetUserIdAsync(member.ApplicationUser);
                var email = await _userManager.GetEmailAsync(member.ApplicationUser);
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(member.ApplicationUser);
                var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(member.ApplicationUser);
                
                emailCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailCode));
                passwordCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordCode));             
                var fullUrl = this.Url.Action("SetPassword", "Account", new { userId = userId, emailCode = emailCode, passwordCode = passwordCode}, this.Request.Scheme);

                MailManager mailManager = new MailManager();
                mailManager.SendEmail(
                "Bestätige deine Registrierung",
                 $"Bitte bestätige deine Registrierung <a href='{HtmlEncoder.Default.Encode(fullUrl)}'>Hier klicken</a>.",
                 email,
                member.ApplicationUser.UserName
                );
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult UpdateMember(Member member)
        {
            _bobContext.Update(member);
            _bobContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult EditMember(string MemberID)
        {
            return PartialView("_UpdateMember", _bobContext.Members.Single(m => m.ID.Equals(MemberID)));
        }

        [HttpDelete]
        public IActionResult DeleteMember(string MemberID)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var member = _bobContext.Members.Include(i => i.ApplicationUser).Single(m => m.ID.Equals(id));
            _bobContext.Members.Remove(member);
            try
            {
                var deletedUser = _bobContext.Users.SingleOrDefault(m => m.Id.Equals(member.ApplicationUser.Id));
                _userManager.DeleteAsync(deletedUser);
            }
            catch (Exception)
            {

            }

            _bobContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CsvExport()
        {
            var allMembers = _bobContext.Members.ToList();
            CSVExportManager.CreateMemberCSV(CSVExportManager.MapMemberToModelMember(allMembers), "wwwroot/export/export.csv");

            var net = new System.Net.WebClient();
            var data = net.DownloadData("wwwroot/export/export.csv");
            var content = new System.IO.MemoryStream(data);
            var contentType = "application/vnd.ms-excel";
            var fileName = "export.csv";

            return File(content, contentType, fileName);
        }

        [HttpGet]
        public async Task<IActionResult> PdfExport()
        {
            var memberListServiceURL = "https://seniorenbobspdfservice.azurewebsites.net/PDFCreate/CreateMemberListPDF";
            var members = await _bobContext.Members.ToListAsync();
            var model = ModelCreators.CreateMemberListPDFModel(members);

            var postResult = await RequestHelper.SendPDFRequestAsync(memberListServiceURL, model);

            if (postResult.StatusCode == HttpStatusCode.OK)
            {
                var content = await RequestHelper.GetPDFContentAsync(postResult);
                return File(content, "application/pdf", "Mitgliederliste.pdf");
            }
            else
            {
                return BadRequest(postResult.Content);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CsvImport(IFormFile file)
        {

            if (file == null)
            {
                throw new FileNotFoundException("Fehler beim einlesen der CSV-Datei.");
            }
            if (!Path.GetExtension(file.FileName).EndsWith(".csv"))
            {
                throw new FileNotFoundException("Es handelt sich nicht um eine CSV-Datei.");
            }

            var fileName = System.IO.Path.GetFileName(file.FileName);
            var path = "wwwroot/export/" + fileName;

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            using (var localFile = System.IO.File.OpenWrite(path))
            using (var uploadedFile = file.OpenReadStream())
            {
                uploadedFile.CopyTo(localFile);
            }

            foreach (var modelMember in CSVExportManager.ReadMemberCSV(path).Users)
            {
                _bobContext.Add(CSVExportManager.MapModelMemberToMember(modelMember, Guid.NewGuid().ToString()));
            }
            _bobContext.SaveChanges();

            System.IO.File.Delete(path);

            var allMembers = _bobContext.Members.ToList();
            return RedirectToAction("Index");
        }
 
        private async Task createRolesandUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new ApplicationRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   
                var user = new ApplicationUser();
                user.UserName = "default";
                user.Email = "default@default.com";
                string userPWD = "somepassword";
                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "Admin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                var role = new ApplicationRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("StandardUser");
            if (!x)
            {
                var role = new ApplicationRole();
                role.Name = "Employee";
                await _roleManager.CreateAsync(role);
            }
        }

    }
}