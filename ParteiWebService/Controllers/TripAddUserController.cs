using ParteiWebService.ViewModel;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Bson.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ParteiWebService.Controllers
{
    public class TripAddUserController : Controller
    {
        private readonly ParteiDbContext _parteiDbContext;

        public TripAddUserController(ParteiDbContext bobcontext)
        {
            _parteiDbContext = bobcontext;
        }
        public IActionResult Index(int TravelId)
        {

            var tid = TravelId;

            TravelMember trav3 = new TravelMember
            {
                TargetCosts = 20,
                ActualCosts = 0,
            };

            List<Stop> stops = _parteiDbContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(TravelId)).First()).ToList();
            stops.Add(_parteiDbContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));
            List<Member> allMembers = _parteiDbContext.Members.ToList();
            List<ExternalMember> allExternalMembers = _parteiDbContext.ExternalMembers.ToList();
            List<TravelMember> allTravelMembers = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).ToList();
            List<ExternalTravelMember> allExternalTravelMembers = _parteiDbContext.ExternalTravelMembers.ToList();

            List<string> allMemberstr = _parteiDbContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_parteiDbContext.Members.Single(x => x.ID.Equals(id)));
            }
            var travel = _parteiDbContext.Travels.Single(x => x.TravelId == TravelId);

            List<string> allExternalMemberstr = _parteiDbContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _parteiDbContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_parteiDbContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }

            var testStops = _parteiDbContext.Stops.ToList();

            var tupleModel = new Tuple<List<Member>, List<ExternalMember>, List<TravelMember>, Travel, List<Stop>, List<ExternalTravelMember>>(mem, allExternalMembers, allTravelMembers, travel, testStops, allExternalTravelMembers);
          
            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = mem,
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = allTravelMembers,
                Travel = travel,
                Stops = testStops,
                ExternalTravelMembers = allExternalTravelMembers
            };

            return View(tripAddUserViewModel);
            //return View(tupleModel);
        }

        [HttpPost]
        public IActionResult AddExternalMember(ExternalMember externalMember, Travel travel)
        {
            externalMember.ID = Guid.NewGuid().ToString();
            _parteiDbContext.Add(externalMember);
            _parteiDbContext.SaveChanges();

            int travelIdreturn = travel.TravelId;

            return RedirectToAction("Index", new { TravelId = travelIdreturn });
        }

        [HttpPost]
        public IActionResult AddMemberToTrip(Member member)
        {
            TravelMember travelMember = new TravelMember();

            travelMember.Member = member;
            _parteiDbContext.Add(travelMember);
            _parteiDbContext.SaveChanges();


            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(TravelMember travelMember)
        {
            /*
             * Das Funkt so noch nicht
             * */
            //var travelMember = 
            _parteiDbContext.Remove(travelMember.ID);
            _parteiDbContext.SaveChanges();
            return RedirectToAction("Index");
        }


        public IActionResult GetTravelMemberData(string MemberID, int travelId)
        {
            //var travelId = Convert.ToInt32(TravelId);
            var Member = _parteiDbContext.Members.Single(member => member.ID.Equals(MemberID));

            if (_parteiDbContext.TravelMembers.Count(x => x.Member.Equals(Member)) > 1)
            {
                throw new Exception("Travelmember existiert bereits");
            }

            var TravelMember = new TravelMember
            {
                ActualCosts = 0,
                Member = _parteiDbContext.Members.Single(member => member.ID.Equals(MemberID)),
                Stop = _parteiDbContext.Stops.Single(stop => stop.StopId.Equals(-1)),
                TargetCosts = _parteiDbContext.Travels.Single(x => x.TravelId.Equals(travelId)).Costs,
                Travel = _parteiDbContext.Travels.Single(travel => travel.TravelId.Equals(travelId)),

            };

            var t = _parteiDbContext.TravelMembers.Add(TravelMember);
            _parteiDbContext.SaveChanges();


            List<string> allMemberstr = _parteiDbContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_parteiDbContext.Members.Single(x => x.ID.Equals(id)));
            }
            List<Stop> stops = _parteiDbContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();
            stops.Add(_parteiDbContext.Stops.SingleOrDefault(x => x.StopId.Equals(-1)));
            var member = _parteiDbContext.TravelMembers.Include(x => x.Member).Where(x => x.Travel.TravelId.Equals(travelId)).ToList();
            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = mem,
                ExternalMembers = null,
                SelectedExternalMemeberIDs = null,
                TravelMembers = member,
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId.Equals(travelId)),
                Stops = stops,
                ExternalTravelMembers = null
            };

            // ViewModel anstatt Tuple
            //return PartialView("_TravelMemberTable", new Tuple<List<TravelMember>, List<Stop>>(member,_parteiDbContext.Stops.ToList()));
            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult DeleteTravelMemberData(string MemberID, int travelId)
        {
            var tm = _parteiDbContext.TravelMembers.Single(x => x.Member.ID.Equals(MemberID) && x.Travel.TravelId.Equals(travelId));

            var t = _parteiDbContext.TravelMembers.Remove(tm);
            _parteiDbContext.SaveChanges();

            List<string> allMemberstr = _parteiDbContext.Members.Select(x => x.ID).ToList();
            List<string> allTravelMembersStr = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.Member.ID).ToList();
            List<string> s = allMemberstr.Except(allTravelMembersStr).ToList();
            List<Member> mem = new List<Member>();
            foreach (string id in s)
            {
                mem.Add(_parteiDbContext.Members.Single(x => x.ID.Equals(id)));
            }

            return PartialView("_MemberTable", mem);
        }

        [HttpGet]
        public IActionResult GetExternalTravelMemberData(string ExternalMemberID, string TravelId, int StopId)
        {
            var travelId = Convert.ToInt32(TravelId);
            var externalMember = _parteiDbContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(ExternalMemberID));

            if (_parteiDbContext.ExternalTravelMembers.Count(x => x.ExternalMember.Equals(externalMember)) > 1)
            {
                throw new Exception("ExternalTravelMember existiert bereits");
            }

            var externalTravelMember = new ExternalTravelMember
            {
                ActualCosts = 0,
                ExternalMember = _parteiDbContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(ExternalMemberID)),
                Stop = _parteiDbContext.Stops.Single(stop => stop.StopId.Equals(StopId)),
                TargetCosts = _parteiDbContext.Travels.Single(x => x.TravelId.Equals(travelId)).Costs,
                Travel = _parteiDbContext.Travels.Single(travel => travel.TravelId.Equals(travelId)),

            };

            var t = _parteiDbContext.ExternalTravelMembers.Add(externalTravelMember);
            _parteiDbContext.SaveChanges();
            var externalMemberOne = _parteiDbContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = _parteiDbContext.ExternalMembers.ToList(),
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == travelId),
                Stops = _parteiDbContext.Stops.ToList(),
                ExternalTravelMembers = externalMemberOne
            };

            return PartialView("_ExternalTravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult DeleteExternalTravelMemberData(string ExternalMemberID, string TravelId)
        {
            
            var travelId = Convert.ToInt32(TravelId);
            var tm = _parteiDbContext.ExternalTravelMembers.Single(x => x.ExternalMember.ID.Equals(ExternalMemberID) && x.Travel.TravelId.Equals(travelId));
            var t = _parteiDbContext.ExternalTravelMembers.Remove(tm);
            _parteiDbContext.SaveChanges();

            List<string> allExternalMemberstr = _parteiDbContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _parteiDbContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(TravelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_parteiDbContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }
            List<Stop> stops = _parteiDbContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();

            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = _parteiDbContext.ExternalTravelMembers.ToList()
            };

            return PartialView("_ExternalTravelMemberTable", tripAddUserViewModel);          
        }

        public IActionResult AddExternalMemberToTrip (TripAddUserViewModel tripAddUserViewModel)
        {
            int travelId = tripAddUserViewModel.Travel.TravelId;
            int stopId = tripAddUserViewModel.Stops[0].StopId;
            

            foreach (var userId in tripAddUserViewModel.SelectedExternalMemeberIDs)
            {
                var externalMember = _parteiDbContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(userId));
                Console.WriteLine(externalMember);

                if (_parteiDbContext.ExternalTravelMembers.Count(x => x.ExternalMember.Equals(externalMember)) > 1)
                {
                    throw new Exception("ExternalTravelMember existiert bereits");
                }
              

                var externalTravelMember = new ExternalTravelMember
                {
                    ActualCosts = 0,
                    ExternalMember = _parteiDbContext.ExternalMembers.Single(externalMember => externalMember.ID.Equals(userId)),
                    Stop = _parteiDbContext.Stops.Single(stop => stop.StopId.Equals(stopId)),
                    TargetCosts = _parteiDbContext.Travels.Single(x => x.TravelId.Equals(travelId)).Costs,
                    Travel = _parteiDbContext.Travels.Single(travel => travel.TravelId.Equals(travelId)),

                };

                var t = _parteiDbContext.ExternalTravelMembers.Add(externalTravelMember);
                _parteiDbContext.SaveChanges();

            }
            var externalMemberOne = _parteiDbContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList();

            List<string> allExternalMemberstr = _parteiDbContext.ExternalMembers.Select(x => x.ID).ToList();
            List<string> allExternalTravelMembersStr = _parteiDbContext.ExternalTravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x => x.ExternalMember.ID).ToList();
            List<string> es = allExternalMemberstr.Except(allExternalTravelMembersStr).ToList();
            List<ExternalMember> exmem = new List<ExternalMember>();
            foreach (string id in es)
            {
                exmem.Add(_parteiDbContext.ExternalMembers.Single(x => x.ID.Equals(id)));
            }
            List<Stop> stops = _parteiDbContext.Stops.Include(x => x.TravelStops).Where(s => s.TravelStops.Select(x => x.TravelId.Equals(travelId)).First()).ToList();

            var tripAddUserViewModelOut = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = exmem,
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == travelId),
                Stops = stops,
                ExternalTravelMembers = externalMemberOne
            };

            return RedirectToAction("Index", new { TravelId = travelId });
            // bei der PartialView hängt der sich iwie auf und erstellt die dann als einzelene View
            //return PartialView("_ExternalTravelMemberTable", tripAddUserViewModelOut);            
        }


        public IActionResult UpdateStop(string MemberID, int travelId, int stopId)
        {

            TravelMember x = _parteiDbContext.TravelMembers.SingleOrDefault(x => x.Travel.TravelId.Equals(travelId) && x.Member.ID.Equals(MemberID));

            x.StopId = stopId;

            _parteiDbContext.Update(x);
            _parteiDbContext.SaveChanges();


            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = _parteiDbContext.ExternalMembers.ToList(),
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == travelId),
                Stops = _parteiDbContext.Stops.ToList(),
                ExternalTravelMembers = _parteiDbContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList(),
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }

        public IActionResult UpdateStopExternal(string ExternalMemberId, int travelId, int stopId)
        {

            ExternalTravelMember x = _parteiDbContext.ExternalTravelMembers.SingleOrDefault(x => x.Travel.TravelId.Equals(travelId) && x.ExternalMember.ID.Equals(ExternalMemberId));

            x.StopId = stopId;

            _parteiDbContext.Update(x);
            _parteiDbContext.SaveChanges();


            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = _parteiDbContext.ExternalMembers.ToList(),
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == travelId),
                Stops = _parteiDbContext.Stops.ToList(),
                ExternalTravelMembers = _parteiDbContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList(),
            };

            return PartialView("_ExternalTravelMemberTable", tripAddUserViewModel);
        }

        [HttpGet]
        public IActionResult UpdateTravelerCard(int travelId)
        {

            var travel = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Count();
            var externTraveler = _parteiDbContext.ExternalMembers.Count();
            var maxTravel = _parteiDbContext.Travels.FirstOrDefault().MaxTraveler;

            Tuple<int, int> tupel = new Tuple<int, int>(travel, maxTravel);

            return PartialView("_TravelParticipantsCard", tupel);
        }    
        
        [HttpGet]
        public IActionResult UpdateTravelCostCard(int travelId)
        {

            var actualCosts = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Sum(x=>x.ActualCosts);
            var targetCosts = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travelId)).Select(x=>x.TargetCosts).FirstOrDefault();
            var maxTravel = _parteiDbContext.Travels.FirstOrDefault().MaxTraveler;
            
            Tuple<double, double> tupel = new Tuple<double, double>(actualCosts, (maxTravel* targetCosts));

            return PartialView("_TravelCostCard", tupel);
        }

        [HttpGet]
        public IActionResult UpdateTravelCost(int travelMemberId,string paid)
        {

            TravelMember x = _parteiDbContext.TravelMembers.SingleOrDefault(x=>x.ID.Equals(travelMemberId));
            if (paid.Equals("true"))
            {
                x.ActualCosts = x.TargetCosts;
            }
            else
            {
                x.ActualCosts = 0;
            }

            _parteiDbContext.Update(x);
            _parteiDbContext.SaveChanges();


            var tripAddUserViewModel = new TripAddUserViewModel
            {
                Members = _parteiDbContext.Members.ToList(),
                ExternalMembers = _parteiDbContext.ExternalMembers.ToList(),
                SelectedExternalMemeberIDs = new List<String>(),
                TravelMembers = _parteiDbContext.TravelMembers.ToList(),
                Travel = _parteiDbContext.Travels.Single(x => x.TravelId == 1),
                Stops = _parteiDbContext.Stops.ToList(),
                ExternalTravelMembers = _parteiDbContext.ExternalTravelMembers.Include(x => x.ExternalMember).ToList(),
            };

            return PartialView("_TravelMemberTable", tripAddUserViewModel);
        }
    }




}