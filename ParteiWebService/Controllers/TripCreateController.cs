using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aufgabe_2.StorageManagers;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;

namespace Aufgabe_2.Controllers
{
    public class TripCreateController : Controller
    {
        private readonly BobContext _bobContext;

        public TripCreateController (BobContext bobContext)
        {
            _bobContext = bobContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddTravelAsync(Travel travel)
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
                            var FileName = file.FileName;
                            var FileSize = int.Parse(file.Length.ToString());
                            var FileType = file.ContentType;
                            var ImageUrl = (String)result.Payload;

                           
                            travel.Images.Add(new Image
                            {
                                ImageUrl = ImageUrl,
                                ImageName = FileName,
                                ImageFileSize = FileSize,
                                ImageFileType = FileType,
                            });
                           
                        }
                    }
                }
            }

            if (travel.Images.Count == 0)
            {
                travel.Images.Add(new Image
                {
                    ImageUrl = null,
                    ImageName = "Kein Foto hochgeladen",
                    ImageFileSize = 0,
                    ImageFileType = null,

                });
            }
            Console.WriteLine(travel.Description);
            
            _bobContext.Add(travel);

            _bobContext.SaveChanges();

            return RedirectToAction("Index","TripOverview");
        }
    }
}