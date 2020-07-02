using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParteiWebService.ViewModel;
using DataAccessLibrary.DataAccess;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DataAccessLibrary.Models;

namespace ParteiWebService.Controllers
{
    public class TripOverviewController : Controller
    {

        private readonly ParteiDbContext _parteiDbContext;
        private readonly UserManager<ApplicationUser> _userManager;


        public TripOverviewController(UserManager<ApplicationUser> userManager, ParteiDbContext parteiDbContext)
        {
            _userManager = userManager;
            _parteiDbContext = parteiDbContext;;
        }
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            
            List<Travel> past = _parteiDbContext.Travels.Include(x => x.Images).Include(x => x.TravelMembers).ThenInclude(m =>m.Member).ThenInclude(a=> a.ApplicationUser).Where(x=>x.StartDate < DateTime.Today && x.OrganizationId == user.OrgranizationId).OrderBy(x => x.StartDate).ToList();
            List<Travel>  upcoming = _parteiDbContext.Travels.Include(x => x.Images).Include(x => x.TravelMembers).ThenInclude(m => m.Member).ThenInclude(a => a.ApplicationUser).Where(x => x.StartDate >= DateTime.Today && x.OrganizationId == user.OrgranizationId).OrderBy(x => x.StartDate).ToList();
            
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
            var travel = _parteiDbContext.Travels.Single(m => m.TravelId == TravelId);
            var user = _userManager.GetUserAsync(User).Result;
            user = _parteiDbContext.ApplicationUsers.Include(m => m.Member).Single(a => a.Id.Equals(user.Id));
            var member = _parteiDbContext.Members.Single(m => m.ID.Equals(user.Member.ID));

            travel.TravelMembers.Add(new TravelMember() { Travel = travel, Member = member});
            _parteiDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult DeleteTravelMember(int TravelId)
        {
          
            var travel = _parteiDbContext.Travels.Single(m => m.TravelId == TravelId);
            var user = _userManager.GetUserAsync(User).Result;
            user = _parteiDbContext.ApplicationUsers.Include(m => m.Member).Single(a => a.Id.Equals(user.Id));
            var member = _parteiDbContext.Members.Single(m => m.ID.Equals(user.Member.ID));

            var travelMember = _parteiDbContext.TravelMembers.Single(m => m.Member.ID == member.ID && m.Travel.TravelId == travel.TravelId);
            _parteiDbContext.Remove(travelMember);

            _parteiDbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult DeleteTravel(int travelId)
        {

            var travel = _parteiDbContext.Travels.Where(x => x.TravelId == travelId).ToList();

            if(travel.Count > 1)
            {
                throw new Exception("Fehler beim löschen! Mehrere Elemente mit der selben ID.");
            }
            if(travel == null)
            {
                throw new Exception("Fehler beim löschen! Keine Elemente gefunden.");
            }
            _parteiDbContext.Travels.Remove(travel[0]);

            return RedirectToAction("Index", "TripOverview");
        }
        [HttpGet]
        public IActionResult UpdateCostProgrssbar(int travelId)
        {

            var travel = _parteiDbContext.Travels.Where(x => x.TravelId == travelId).ToList();


            return PartialView("_CostProgressbar", travel);
        }

  

    }
}