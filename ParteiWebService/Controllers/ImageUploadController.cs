using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Aufgabe_2.Models;
using Microsoft.AspNetCore.Hosting;
using Aufgabe_2.StorageManagers;
using DataAccessLibrary.Models;
using DataAccessLibrary.DataAccess;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Aufgabe_2.Controllers
{
    public class ImageUploadController : Controller
    {
        private readonly ILogger<ImageUploadController> _logger;
        private readonly IHostingEnvironment _environment;
        private readonly BobContext _bobContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ImageUploadController(ILogger<ImageUploadController> logger, IHostingEnvironment environment,
            BobContext bobContext,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _environment = environment;
            _logger = logger;
            _bobContext = bobContext;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        /*
        [Authorize]
        [HttpGet]
        public IActionResult Index()
        {
            var mongoCollection = CosmosManager.ImageDataCollection;
            var findResult = CosmosManager.ImageDataCollection.Find(e => e.Id != null).ToList();
            return View(findResult);
        }

        private void AddOrganization()
        {
            _bobContext.Organizations.Add(new Organization
            {
                Name = "Piraten3"
            });

            _bobContext.SaveChanges();
            var orgas = _bobContext.Organizations.ToList();
            var org = _bobContext.Organizations.Where<Organization>(o => o.Name == "Piraten").First<Organization>();

            org.Members.Add(new Member
            {
                ID = Guid.NewGuid().ToString(),
                LastName = "Schmidt",
                PreName = "Moritz"
            });

            _bobContext.SaveChanges();

            org = _bobContext.Organizations.Where<Organization>(o => o.Name == "Piraten").First<Organization>();

            _bobContext.Organizations.Remove(org);

            _bobContext.SaveChanges();

            // Muss null sein, da entfernt..
            org = _bobContext.Organizations.Where<Organization>(o => o.Name == "Piraten").FirstOrDefault<Organization>();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            if (HttpContext.Request.Form.Files != null)
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var file in files)
                {
                    if (file.Length > 0)
                    {
                        var result = await BlobManager.AddImageAsync(file.FileName, file);

                        if (result.Successfull)
                        {
                            CosmosManager.ImageDataCollection.InsertOne(new ImageDataModel
                            {
                                FileName = file.FileName,
                                FileSize = int.Parse(file.Length.ToString()),
                                FileType = file.ContentType,
                                ImageUrl = (String) result.Payload,
                                LastModified = DateTime.Now.ToString()
                            }); ;
                        }
                    }
                }
            }
            var mongoCollection = CosmosManager.ImageDataCollection;
            var findResult = CosmosManager.ImageDataCollection.Find(e => e.Id != null).ToList();
            return View(findResult);
        }

        */
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }     
    }
}
