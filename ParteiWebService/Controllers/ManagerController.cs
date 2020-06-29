using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using ParteiWebService.CSV_Export;
using ParteiWebService.MicroServiceHelpers.PDFService;
using ParteiWebService.Models;
using ParteiWebService.Utility;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace ParteiWebService.Controllers
{
    public class ManagerController : Controller
    {

        public ParteiDbContext _parteiDbContext { get; }
        private readonly UserManager<ApplicationUser> _userManager;


        public ManagerController(ParteiDbContext parteiDbContext, UserManager<ApplicationUser> userManager)
        {
            _parteiDbContext = parteiDbContext;
            _userManager = userManager;
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var organizsationUser = _parteiDbContext.Organizations.Where(u => u.Admin.Id.Equals(user.Id)).SingleOrDefault();

            var memberSelectList = new ApplicationUserMultiselectModel()
            {
                ApplicationUsers = _parteiDbContext.ApplicationUsers.Include(a => a.UserRoles).ThenInclude(a => a.Role).Include(m => m.Member).Where(a => a.Member.Organization.Name.Equals(organizsationUser.Name)).ToList(),
                SelectedMemberIDs = new List<String>()
            };

            return View(memberSelectList);
        }

        [HttpDelete]
        public IActionResult DeleteMember(string MemberID)
        {
            return RedirectToAction("Index");
        }
        [HttpPost]
        public async Task<IActionResult> AddManagerAsync(ApplicationUserMultiselectModel applicationUserMultiselectModel)
        {
            foreach (var userId in applicationUserMultiselectModel.SelectedMemberIDs)
            {
                var user = await _userManager.FindByIdAsync(userId);
                await _userManager.AddToRoleAsync(user, "Manager");
            }

            return RedirectToAction("Index");
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
                _parteiDbContext.Add(CSVExportManager.MapModelMemberToMember(modelMember, Guid.NewGuid().ToString()));
            }
            _parteiDbContext.SaveChanges();

            System.IO.File.Delete(path);

            var allMembers = _parteiDbContext.Members.ToList();
            return RedirectToAction("Index");
        }

    }
}
