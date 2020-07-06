using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ParteiWebService.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.Controllers
{
    public class TripCreateController : Controller
    {
        private readonly ParteiDbContext _parteiDbContext;
        private readonly UserManager<ApplicationUser> _userManager;

        public TripCreateController(ParteiDbContext parteiDbContext, UserManager<ApplicationUser> userManager)
        {
            _parteiDbContext = parteiDbContext;;
            _userManager = userManager;
        }

        public async Task<IActionResult> IndexAsync()
        {
            var user =  await _userManager.GetUserAsync(User);

            var tripCreateViewModel = new TripCreateViewModel()
            {
                Travel = new Travel() { OrganizationId = user.OrgranizationId},
                StopList = _parteiDbContext.Stops.Where(x=>!x.StopId.Equals(-1)).ToList(),
                SelectedStops = new List<int>(),
            };
            return View(tripCreateViewModel);
        }

        public IActionResult UpdateTravel(int tripId)
        {
            var travel = _parteiDbContext.Travels.Find(tripId);
            var stopList = _parteiDbContext.TravelStops.Where(x => x.TravelId == tripId).Select(x => x.StopId).ToList();
            var tripCreateViewModel = new TripCreateViewModel()
            {
                Travel = travel,
                StopList = _parteiDbContext.Stops.ToList(),
                SelectedStops = stopList,
            };
            return View(tripCreateViewModel);
        }

        [HttpPost]
        public IActionResult UpdateTravel(TripCreateViewModel tripCreateViewModel)
        {
            _parteiDbContext.Travels.Update(tripCreateViewModel.Travel);           
            var travel = _parteiDbContext.Travels.Include(t => t.TravelStops).Single(t => t.TravelId == tripCreateViewModel.Travel.TravelId);
            _parteiDbContext.TravelStops.RemoveRange(travel.TravelStops);
            _parteiDbContext.SaveChanges();

            foreach (int stop in tripCreateViewModel.SelectedStops)
            {
                TravelStop travelStop = new TravelStop
                {
                    TravelId = tripCreateViewModel.Travel.TravelId,
                    StopId = stop,
                };
                _parteiDbContext.Add(travelStop);
            }
            _parteiDbContext.SaveChanges();

            return RedirectToAction("Index", "TripOverview");
        }


        [HttpPost]
        public async Task<IActionResult> AddTravelAsync(TripCreateViewModel tripCreateViewModel)
        {
            //if (HttpContext.Request.Form.Files != null)
            //{
            //    var files = HttpContext.Request.Form.Files;
            //    foreach (var file in files)
            //    {
            //        if (file.Length > 0)
            //        {
            //            var result = await BlobManager.AddImageAsync(file.FileName, file);

            //            if (result.Successfull)
            //            {
            //                var FileName = file.FileName;
            //                var FileSize = int.Parse(file.Length.ToString());
            //                var FileType = file.ContentType;
            //                var ImageUrl = (String)result.Payload;


            //                tripCreateViewModel.Travel.Images.Add(new Image
            //                {
            //                    ImageUrl = ImageUrl,
            //                    ImageName = FileName,
            //                    ImageFileSize = FileSize,
            //                    ImageFileType = FileType,
            //                });

            //            }
            //        }
            //    }
            //}

            //if (tripCreateViewModel.Travel.Images.Count == 0)
            //{
            //    tripCreateViewModel.Travel.Images.Add(new Image
            //    {
            //        ImageUrl = null,
            //        ImageName = "Kein Foto hochgeladen",
            //        ImageFileSize = 0,
            //        ImageFileType = null,

            //    });
            //}

            Console.WriteLine(tripCreateViewModel.Travel.Description);
            _parteiDbContext.Add(tripCreateViewModel.Travel);
            _parteiDbContext.SaveChanges();

            foreach (int stop in tripCreateViewModel.SelectedStops)
            {
                TravelStop travelStop = new TravelStop
                {
                    TravelId = tripCreateViewModel.Travel.TravelId,
                    StopId = stop,
                };
                _parteiDbContext.Add(travelStop);
            }

            _parteiDbContext.SaveChanges();

            return RedirectToAction("Index", "TripOverview");
        }

        public IActionResult StopList(String stopName)
        {
            if (_parteiDbContext.Stops.Where(x => x.StopName.ToLower().Equals(stopName.ToLower())).Count() >= 1)
            {
                throw new ArgumentException("Element existiert bereits!");
            }

            _parteiDbContext.Add(new Stop()
            {
                StopName=stopName,
            });
            _parteiDbContext.SaveChanges();


            var tripCreateViewModel = new TripCreateViewModel()
            {
                StopList = _parteiDbContext.Stops.Where(x => !x.StopId.Equals(-1)).ToList(),
                SelectedStops = new List<int>(),
            };

            return PartialView("_SelectStopList", tripCreateViewModel);
        }


    }
}