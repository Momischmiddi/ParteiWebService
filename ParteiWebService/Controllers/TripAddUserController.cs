using Aufgabe_2.ViewModel;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;

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

            TravelMember trav3 = new TravelMember
            {
                TargetCosts = 20,
                ActualCosts = 0,
            };

            List<Stop> stops = _bobContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(TravelId)).First()).ToList();
            //stops.Add(_bobContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));

            List<Member> allMembers = _bobContext.Members.ToList();
            List<ExternalMember> allExternalMembers = _bobContext.ExternalMembers.ToList();
            List<TravelMember> allTravelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).ToList();           
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).ToList();

            List<string> allMemberstr = _bobContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_bobContext.Members.Single(x => x.ID.Equals(id)));
            }
            var travel = _bobContext.Travels.Single(x => x.TravelId == TravelId);

            List<string> allExternalMemberstr = _bobContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_bobContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }

           // var testStops = _bobContext.Stops.ToList();

            //var tupleModel = new Tuple<List<Member>, List<ExternalMember>, List<TravelMember>, Travel, List<Stop>, List<ExternalTravelMember>>(mem, allExternalMembers, allTravelMembers, travel, testStops, allExternalTravelMembers);
          
            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = mem,
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = allTravelMembers,
                Travel = travel,
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };

            return View(tripAddUserViewModel);
            //return View(tupleModel);
        }

        [HttpPost]
        public IActionResult AddExternalMember(ExternalMember externalMember, Travel travel)
        {
            externalMember.ID = Guid.NewGuid().ToString();
            _bobContext.Add(externalMember);
            _bobContext.SaveChanges();

            int travelIdreturn = travel.TravelId;

            return RedirectToAction("Index", new { TravelId = travelIdreturn });
        }

        [HttpPost]
        public IActionResult AddMemberToTrip(Member member)
        {
            TravelMember travelMember = new TravelMember();

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


        public IActionResult GetTravelMemberData(string MemberID, int travelId)
        {
            //var travelId = Convert.ToInt32(TravelId);
            var Member = _bobContext.Members.Single(member => member.ID.Equals(MemberID));

            if (_bobContext.TravelMembers.Count(x => x.Member.Equals(Member)) > 1)
            {
                throw new Exception("Travelmember existiert bereits");
            }
                      
            var travel = _bobContext.Travels.Single(travel => travel.TravelId.Equals(travelId));

            var TravelMember = new TravelMember
            {
                ActualCosts = 0,
                Member = Member,                
                Travel = travel
            };

            Console.WriteLine(TravelMember);
            var t = _bobContext.TravelMembers.Add(TravelMember);
            _bobContext.SaveChanges();


            List<string> allMemberstr = _bobContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_bobContext.Members.Single(x => x.ID.Equals(id)));
            }
            List<Stop> stops = _bobContext.Stops.ToList();
            var member = _bobContext.TravelMembers.Include(x => x.Member).Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = mem,
                ExternalMembers = null,
                SelectedExternalMemeberIDs = null,
                TravelMembers = member,
                Travel = _bobContext.Travels.Single(x => x.TravelId.Equals(travelId)),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult DeleteTravelMemberData(string MemberID, int travelId)
        {
            var tm = _bobContext.TravelMembers.Single(x => x.Member.ID.Equals(MemberID) && x.Travel.TravelId.Equals(travelId));

            var t = _bobContext.TravelMembers.Remove(tm);
            _bobContext.SaveChanges();

            List<string> allMemberstr = _bobContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_bobContext.Members.Single(x => x.ID.Equals(id)));
            }

            var member = _bobContext.TravelMembers.Include(x => x.Member).Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<Stop> stops = _bobContext.Stops.ToList();
            //stops.Add(_bobContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));

            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = mem,
                ExternalMembers = externalMembers,
                SelectedExternalMemeberIDs = null,
                TravelMembers = member,
                Travel = _bobContext.Travels.Single(x => x.TravelId.Equals(travelId)),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };
            
            // so wird nur die TravelMemberTable aktualisiert... die MemberTable bleibt aber unverändert und wird halt beim neuladen dann wieder richtig initialisiert!
            return PartialView("_TravelMemberTable", tripAddUserViewModel);
            // return RedirectToAction("Index", new { TravelId = travelId });
        }

        [HttpGet]
        public IActionResult GetMemberData(int travelId)
        {
            List<string> allMemberstr = _bobContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_bobContext.Members.Single(x => x.ID.Equals(id)));
            }

            return PartialView("_MemberTable", mem);
        }

        [HttpGet]
        public IActionResult GetExternalTravelMemberData(string ExternalMemberID, string TravelId)
        {
            var travelId = Convert.ToInt32(TravelId);
            var externalMember = _bobContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(ExternalMemberID));

            if (_bobContext.ExternalTravelMembers.Count(x => x.ExternalMember.Equals(externalMember)) > 1)
            {
                throw new Exception("ExternalTravelMember existiert bereits");
            }

            var externalTravelMember = new ExternalTravelMember
            {
                ActualCosts = 0,
                ExternalMember = _bobContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(ExternalMemberID)),                
                Travel = _bobContext.Travels.Single(travel => travel.TravelId.Equals(travelId)),

            };

            var t = _bobContext.ExternalTravelMembers.Add(externalTravelMember);
            _bobContext.SaveChanges();
            var externalMemberOne = _bobContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList();

            List<Stop> stops = _bobContext.Stops.ToList();
            //stops.Add(_bobContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));
            List<Member> members = _bobContext.Members.ToList();
            List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();
            List<TravelMember> travelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = members,
                ExternalMembers = externalMembers,
                SelectedExternalMemeberIDs = null,
                TravelMembers = travelMembers,
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };


            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult DeleteExternalTravelMemberData(string ExternalMemberID, string TravelId)
        {
            
            var travelId = Convert.ToInt32(TravelId);
            var tm = _bobContext.ExternalTravelMembers.Single(x => x.ExternalMember.ID.Equals(ExternalMemberID) && x.Travel.TravelId.Equals(travelId));
            var t = _bobContext.ExternalTravelMembers.Remove(tm);
            _bobContext.SaveChanges();

            List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();
            List<Stop> stops = _bobContext.Stops.ToList();
            //stops.Add(_bobContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));
            List<TravelMember> travelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<Member> members = _bobContext.Members.ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = members,
                ExternalMembers = externalMembers,
                SelectedExternalMemeberIDs = null,
                TravelMembers = travelMembers,
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);          
        }

        [HttpGet]
        public IActionResult GetExternalMemberData(int travelId)
        {

           // List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();
            List<Stop> stops = _bobContext.Stops.ToList();
            //stops.Add(_bobContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));
            List<TravelMember> travelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<Member> members = _bobContext.Members.ToList();

            List<string> allExternalMemberstr = _bobContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_bobContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = members,
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = null,
                TravelMembers = travelMembers,
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers
            };

            return PartialView("_ExternalMemberDropdown", tripAddUserViewModel);
        }

        public IActionResult AddExternalMemberToTrip (TripAddUserViewModel tripAddUserViewModel)
        {            
            int travelId = tripAddUserViewModel.Travel.TravelId;
            int stopId = tripAddUserViewModel.Stops[0].StopId;
            

            foreach (var userId in tripAddUserViewModel.SelectedExternalMemeberIDs)
            {
                var externalMember = _bobContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(userId));
                Console.WriteLine(externalMember);

                if (_bobContext.ExternalTravelMembers.Count(x => x.ExternalMember.Equals(externalMember)) > 1)
                {
                    throw new Exception("ExternalTravelMember existiert bereits");
                }
              

                var externalTravelMember = new ExternalTravelMember
                {
                    ActualCosts = 0,
                    ExternalMember = _bobContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(userId)),
                    Travel = _bobContext.Travels.Single(travel => travel.TravelId.Equals(travelId)),
                };

                var t = _bobContext.ExternalTravelMembers.Add(externalTravelMember);
                _bobContext.SaveChanges();

            }
            var externalMemberOne = _bobContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList();

            List<string> allExternalMemberstr = _bobContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_bobContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }
            List<Stop> stops = _bobContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();

            var tripAddUserViewModelOut = new TripAddUserViewModel
            {
                Members = _bobContext.Members.ToList(),
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _bobContext.TravelMembers.ToList(),
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalMemberOne
            };

            return RedirectToAction("Index", new { TravelId = travelId });          
        }


        public IActionResult UpdateStop(string MemberID, int travelId, int stopId)
        {

            TravelMember x = _bobContext.TravelMembers.SingleOrDefault(x => x.Travel.TravelId.Equals(travelId) && x.Member.ID.Equals(MemberID));

            x.StopId = stopId;

            _bobContext.Update(x);
            _bobContext.SaveChanges();

            //TODO Die Liste sauber befüllen...

            List<Member> members = _bobContext.Members.ToList();
            List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();
            List<TravelMember> travelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<Stop> stops = _bobContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = members,
                ExternalMembers = externalMembers,
                SelectedExternalMemeberIDs = null,
                TravelMembers = travelMembers,
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers,
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult UpdateStopExternal(string ExternalMemberId, int travelId, int stopId)
        {

            ExternalTravelMember x = _bobContext.ExternalTravelMembers.SingleOrDefault(x => x.Travel.TravelId.Equals(travelId) && x.ExternalMember.ID.Equals(ExternalMemberId));

            x.StopId = stopId;

            _bobContext.Update(x);
            _bobContext.SaveChanges();


            //TODO Die Liste suaber befüllen...
            // Funktion im Frontend dann noch callen!
            List<Member> members = _bobContext.Members.ToList();
            List<ExternalMember> externalMembers = _bobContext.ExternalMembers.ToList();
            List<TravelMember> travelMembers = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<ExternalTravelMember> externalTravelMembers = _bobContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            List<Stop> stops = _bobContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = members,
                ExternalMembers = externalMembers,
                SelectedExternalMemeberIDs = null,
                TravelMembers = travelMembers,
                Travel = _bobContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalTravelMembers,
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        [HttpGet]
        public IActionResult UpdateTravelerCard(int travelId)
        {

            var travel = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Count();
            var externTraveler = _bobContext.ExternalMembers.Count();
            var maxTravel = _bobContext.Travels.FirstOrDefault().MaxTraveler;

            Tuple<int, int> tupel = new Tuple<int, int>(travel, maxTravel);

            return PartialView("_TravelParticipantsCard", tupel);
        }    
        
        [HttpGet]
        public IActionResult UpdateTravelCostCard(int travelId)
        {

            var actualCosts = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Sum(x=>x.ActualCosts);
            var targetCosts = _bobContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x=>x.TargetCosts).FirstOrDefault();
            var maxTravel = _bobContext.Travels.FirstOrDefault().MaxTraveler;
            
            Tuple<double, double> tupel = new Tuple<double, double>(actualCosts, (maxTravel* targetCosts));

            return PartialView("_TravelCostCard", tupel);
        }

        [HttpGet]
        public IActionResult UpdateTravelCost(int travelMemberId,string paid)
        {

            TravelMember x = _bobContext.TravelMembers.SingleOrDefault(x=>x.ID.Equals(travelMemberId));
            if (paid.Equals("true"))
            {
                x.ActualCosts = x.TargetCosts;
            }
            else
            {
                x.ActualCosts = 0;
            }

            _bobContext.Update(x);
            _bobContext.SaveChanges();


            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _bobContext.Members.ToList(),
                ExternalMembers = _bobContext.ExternalMembers.ToList(),
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _bobContext.TravelMembers.ToList(),
                Travel = _bobContext.Travels.Single(x => x.TravelId == 1),
                Stops = _bobContext.Stops.ToList(),
                ExternalTravelMembers = _bobContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList(),
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }
    }




}