using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ParteiWebService.CSV_Export;
using ParteiWebService.Models;
using ParteiWebService.StorageManagers;
using ParteiWebService.ViewModel;

namespace ParteiWebService.Controllers
{
    public class ManagerController : Controller
    {

        public ParteiDbContext _parteiDbContext { get; }
        private readonly UserManager<ApplicationUser> _userManager;


        public ManagerController(ParteiDbContext parteiDbContext, UserManager<ApplicationUser> userManager)
        {
            _parteiDbContext = parteiDbContext;;
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> SaveImage(IFormFile organizationImage)
        {
            if (HttpContext.Request.Form.Files != null)
            {
                var files = HttpContext.Request.Form.Files;
                var result = await BlobManager.AddImageAsync(organizationImage.FileName, organizationImage);
                var payload = result.Payload;
                var user = await _userManager.GetUserAsync(User);
                var orga = _parteiDbContext.Organizations.Single(x => x.Id == user.OrgranizationId);
                orga.OrganizationImage = payload.ToString();
                _parteiDbContext.SaveChanges();
            }
            return RedirectToAction("Index");
        }


        [Authorize]
        [HttpGet]
        public async Task<IActionResult> IndexAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            var memberSelectList = new ApplicationUserMultiselectModel()
            {
                ApplicationUsers = _parteiDbContext.ApplicationUsers.Include(a => a.UserRoles).ThenInclude(a => a.Role).Include(m => m.Member).Where(a => a.Member.OrganizationId == user.OrgranizationId).ToList(),
                SelectedMemberIDs = new List<String>()
            };

            var managerModel = new ManagerViewModel()
            {
                ApplicationUserMultiselectModel = memberSelectList,
                Organization = _parteiDbContext.Organizations.Single(o => o.Id == user.OrgranizationId)
            }; 
            return View(managerModel);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.RemoveFromRoleAsync(user, "Manager");
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
            int organisationId = 2; // TODO hier noch die Anpassung machen!!!


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
                _parteiDbContext.Add(CSVExportManager.MapModelMemberToMember(modelMember, Guid.NewGuid().ToString(), organisationId));
            }
            _parteiDbContext.SaveChanges();

            System.IO.File.Delete(path);

            var allMembers = _parteiDbContext.Members.ToList();
            return RedirectToAction("Index");
        }

    }
}
