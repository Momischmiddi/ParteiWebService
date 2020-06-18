using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Aufgabe_2.MicroServiceHelpers.PDFService;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using DataAccessLibrary.Models.DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aufgabe_2.Controllers
{
    public class TripAddUserController : Controller
    {
        private readonly BobContext _bobContext;

        public TripAddUserController(BobContext bobcontext)
        {
            _bobContext = bobcontext;
        }
        public IActionResult Index(int TravelId)
        {

            var tid = TravelId;

            TravelMember trav1 = new TravelMember
            {
                ID = "123-asd-12334",
                TargetCosts = 30,
                ActualCosts = 20,
                Stop = new Stop
                {
                    StopName="Freiburg",
                    
                },
            };
            TravelMember trav2 = new TravelMember
            {
                ID = "123-asd-12334",
                TargetCosts = 30,
                ActualCosts = 20,
                Stop = new Stop
                {
                    StopName = "Freiburg",

                },
            };
            TravelMember trav3 = new TravelMember
            {
                ID = "123-asd-12334",
                TargetCosts = 30,
                ActualCosts = 20,
                Stop = new Stop
                {
                    StopName = "Freiburg",

                },
            };

            List<Member> allMembers = _bobContext.Members.ToList();
            List<ExternalMember> allExternalMembers = _bobContext.ExternalMemebers.ToList();
            //List<TravelMember> allTravelMembers = _bobContext.TravelMembers.ToList();
            List<TravelMember> allTravelMembers = new List<TravelMember>();
            allTravelMembers.Add(trav1);
            allTravelMembers.Add(trav2);
            allTravelMembers.Add(trav3);
            var travel = _bobContext.Travels.Single(x => x.TravelId == TravelId);

            var tupleModel = new Tuple<List<Member>, List<ExternalMember>, List<TravelMember>,Travel>(allMembers, allExternalMembers, allTravelMembers,travel);
            
            return View(tupleModel);
        }

        [HttpGet]
        public async Task<IActionResult> PDFExport(int travelId)
        {
            var memberListServiceURL = "https://seniorenbobspdfservice.azurewebsites.net/PDFCreate/CreateTripPDF";

            var travel = await _bobContext.Travels.Where(tr => tr.TravelId == travelId).FirstAsync();
            var model = ModelCreators.CreateTripPDFModel(travel);

            var postResult = await RequestHelper.SendPDFRequestAsync(memberListServiceURL, model);

            if (postResult.StatusCode == HttpStatusCode.OK)
            {
                var content = await RequestHelper.GetPDFContentAsync(postResult);
                return File(content, "application/pdf", "Reise.pdf");
            }
            else
            {
                return BadRequest(postResult.Content);
            }
        }

        [HttpPost]
        public IActionResult AddExternalMember(ExternalMember externalMember)
        {
            externalMember.ID = Guid.NewGuid().ToString();
            _bobContext.Add(externalMember);
            _bobContext.SaveChanges();

            //return View();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddMemberToTrip (Member member)
        {
            TravelMember travelMember = new TravelMember();
            travelMember.ID = Guid.NewGuid().ToString();
            travelMember.Member = member;
            _bobContext.Add(travelMember);
            _bobContext.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(TravelMember travelMember)
        {
            /*
             * Das Funkt so noch nicht
             * */
            //var travelMember = 
            _bobContext.Remove(travelMember.ID);
            _bobContext.SaveChanges();
            return RedirectToAction("Index");
        }
      

    }
}