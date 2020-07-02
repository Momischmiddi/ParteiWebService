using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aufgabe_2.ViewModel;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Aufgabe_2.Controllers
{
    public class TripOverviewController : Controller
    {

        private readonly BobContext _bobContext;
        private readonly UserManager<ApplicationUser> _userManager;


        public TripOverviewController(UserManager<ApplicationUser> userManager, BobContext bobContext)
        {
            _userManager = userManager;
            _bobContext = bobContext;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            List<Travel> past = _bobContext.Travels.Include(x => x.Images).Include(x => x.TravelMembers).ThenInclude(m =>m.Member).ThenInclude(a=> a.ApplicationUser).Where(x=>x.StartDate < DateTime.Today && x.OrganizationId == user.OrgranizationId).OrderBy(x => x.StartDate).ToList();
            List<Travel>  upcoming = _bobContext.Travels.Include(x => x.Images).Include(x => x.TravelMembers).ThenInclude(m => m.Member).ThenInclude(a => a.ApplicationUser).Where(x => x.StartDate >= DateTime.Today && x.OrganizationId == user.OrgranizationId).OrderBy(x => x.StartDate).ToList();
            
            TripOverviewViewModel tripOverviewViewModel = new TripOverviewViewModel()
            {
                PastTravels = past,
                UpcomingTravels = upcoming
                
            };
            return View(tripOverviewViewModel);
        }

        [HttpGet]
        public IActionResult AddTravelMember(int TravelId)
        {
            var travel = _bobContext.Travels.Single(m => m.TravelId == TravelId);
            var user = _userManager.GetUserAsync(User).Result;
            user = _bobContext.ApplicationUsers.Include(m => m.Member).Single(a => a.Id.Equals(user.Id));
            var member = _bobContext.Members.Single(m => m.ID.Equals(user.Member.ID));

            travel.TravelMembers.Add(new TravelMember() { Travel = travel, Member = member});
            _bobContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteTravelMember(int TravelId)
        {
          
            var travel = _bobContext.Travels.Single(m => m.TravelId == TravelId);
            var user = _userManager.GetUserAsync(User).Result;
            user = _bobContext.ApplicationUsers.Include(m => m.Member).Single(a => a.Id.Equals(user.Id));
            var member = _bobContext.Members.Single(m => m.ID.Equals(user.Member.ID));

            var travelMember = _bobContext.TravelMembers.Single(m => m.Member.ID == member.ID && m.Travel.TravelId == travel.TravelId);
            _bobContext.Remove(travelMember);

            _bobContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTravel(int travelId)
        {

            var travel = _bobContext.Travels.Where(x => x.TravelId == travelId).ToList();

            if(travel.Count > 1)
            {
                throw new Exception("Fehler beim löschen! Mehrere Elemente mit der selben ID.");
            }
            if(travel == null)
            {
                throw new Exception("Fehler beim löschen! Keine Elemente gefunden.");
            }
            _bobContext.Travels.Remove(travel[0]);

            return RedirectToAction("Index", "TripOverview");
        }
        [HttpGet]
        public IActionResult UpdateCostProgrssbar(int travelId)
        {

            var travel = _bobContext.Travels.Where(x => x.TravelId == travelId).ToList();


            return PartialView("_CostProgressbar", travel);
        }

  

    }
}