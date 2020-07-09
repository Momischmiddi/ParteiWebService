using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using ParteiWebService.CSV_Export;
using ParteiWebService.MicroServiceHelpers.PDFService;
using ParteiWebService.Utility;
using ParteiWebService.ViewModel;

namespace ParteiWebService.Controllers
{
    public class HomeController : Controller
    {
        private readonly ParteiDbContext _parteiDbContext;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailSender _emailSender;

        public HomeController(ParteiDbContext parteiDbContext, RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IEmailSender emailSender)
        {
            _emailSender = emailSender;
            _parteiDbContext = parteiDbContext;;
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
            var organizationId = user.OrgranizationId; // TODO an die View mit übergeben!
            var allMembers = _parteiDbContext.Members.Where(a => a.Organization.Id == user.OrgranizationId).ToList();
            return View(allMembers);

        }

        [HttpPost]
        public async Task<IActionResult> AddMemberAsync(Member member)
        {
            member.ID = Guid.NewGuid().ToString();
            var user = await _userManager.GetUserAsync(User);
            member.OrganizationId = user.OrgranizationId;
            if (!member.IsActiveMember)
            {
                member.ApplicationUser = null;
                member.ApplicationUserId = null;
            }
            _parteiDbContext.Add(member);
            _parteiDbContext.SaveChanges();

            if (member.IsActiveMember)
            {
                member.ApplicationUser.OrgranizationId = user.OrgranizationId;

                if (member.ApplicationUser == null)
                {
                    return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
                }
                member.ApplicationUser.UserName = member.ID;
                member.ApplicationUser.PasswordHash = Guid.NewGuid().ToString();
                _parteiDbContext.SaveChanges();

                var userId = await _userManager.GetUserIdAsync(member.ApplicationUser);
                var email = await _userManager.GetEmailAsync(member.ApplicationUser);
                var emailCode = await _userManager.GenerateEmailConfirmationTokenAsync(member.ApplicationUser);
                var passwordCode = await _userManager.GeneratePasswordResetTokenAsync(member.ApplicationUser);

                emailCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(emailCode));
                passwordCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordCode));
                var fullUrl = this.Url.Action("SetPassword", "Account", new { userId = userId, emailCode = emailCode, passwordCode = passwordCode }, this.Request.Scheme);

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
            _parteiDbContext.Update(member);
            _parteiDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult EditMember(string MemberID)
        {
            return PartialView("_UpdateMember", _parteiDbContext.Members.Single(m => m.ID.Equals(MemberID)));
        }

        [HttpDelete]
        public IActionResult DeleteMember(string MemberID)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Delete(string id)
        {
            var member = _parteiDbContext.Members.Include(i => i.ApplicationUser).Single(m => m.ID.Equals(id));
            _parteiDbContext.Members.Remove(member);
            try
            {
                var deletedUser = _parteiDbContext.Users.SingleOrDefault(m => m.Id.Equals(member.ApplicationUser.Id));
                _userManager.DeleteAsync(deletedUser);
            }
            catch (Exception)
            {

            }

            _parteiDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult CsvExport()
        {
            var allMembers = _parteiDbContext.Members.ToList();
            CSVExportManager.CreateMemberCSV(CSVExportManager.MapMemberToModelMember(allMembers), "wwwroot/export/export.csv");

            var net = new System.Net.WebClient();
            var data = net.DownloadData("wwwroot/export/export.csv");
            var content = new System.IO.MemoryStream(data);
            var contentType = "application/vnd.ms-excel";
            var fileName = "export.csv";

            return File(content, contentType, fileName);
        }

        [HttpPost]
        public async Task<IActionResult> PdfExport()
        {
            var members = await _parteiDbContext.Members.ToListAsync();
            var model = ModelCreators.CreateMemberListPDFModel(members);

            var postResult = await RequestHelper.SendPDFRequestAsync(RequestHelper.EndPoint.CreateMemberListPDF ,model);

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
        public async Task<IActionResult> CsvImport(CsvImportExportViewModel homeViewModel)
        {

            var file = homeViewModel.File;
            var organisationId = homeViewModel.OrganisationId;

            if (file == null)
            {
                throw new FileNotFoundException("Fehler beim einlesen der CSV-Datei.");
            }
            if (!Path.GetExtension(file.FileName).EndsWith(".csv"))
            {
                throw new FileNotFoundException("Es handelt sich nicht um eine CSV-Datei.");
            }

            var fileName = Path.GetFileName(file.FileName);
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
                _parteiDbContext.Add(CSVExportManager.MapModelMemberToMember(modelMember, Guid.NewGuid().ToString(), organisationId)); // add OrganisationId
            }
            _parteiDbContext.SaveChanges();

            System.IO.File.Delete(path);

            var allMembers = _parteiDbContext.Members.ToList();
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

        [HttpPost]
        public async Task<IActionResult> SendMailToSomeOneAsync(SendMailViewModel sendMailViewModel)
        {       
            // Was steht im Betreff?
            String subject = "Mitgliederliste";

            // Was soll gesendet werden?
            DateTime today = DateTime.Today;
            String content = "Auszug aus der Mitgliederliste vom " + today; 

            // An wen soll gesendet werden?
            var destinationAddress = sendMailViewModel.DestinationAddress;
            var destinationName = sendMailViewModel.DestinationName;

            var members = await _parteiDbContext.Members.ToListAsync();
            var model = ModelCreators.CreateMemberListPDFModel(members);

            var postResult = await RequestHelper.SendPDFRequestAsync(RequestHelper.EndPoint.CreateMemberListPDF, model);

            if (postResult.StatusCode == HttpStatusCode.OK)
            {
                var mailPdfContent = await RequestHelper.GetPDFContentAsync(postResult);

                MailManager mailManager = new MailManager();
                await mailManager.SendEmail(subject, content, destinationAddress, destinationName, mailPdfContent, "Mitgliederliste.pdf");

                return RedirectToAction("Index");
            }
            else
            {
                return BadRequest(postResult.Content);
            }
                                
        }

    }
}