using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParteiWebService.ViewModel;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ParteiWebService.Controllers
{
    public class TripOverviewController : Controller
    {

        private readonly ParteiDbContext _parteiDbContext;


        public TripOverviewController(ParteiDbContext parteiDbContext)
        {
            _parteiDbContext = parteiDbContext;
        }
        public IActionResult Index()
        {
            /*
            TravelMember travelMember = new TravelMember
            {
                ID = "dd21eb68-96cc-4288-b43f-4783eeea87e5",
            };

            Travel t1 = new Travel
            {
                Destination = "Freiburg",
                Costs = 20.00,
                Description = "Wünderschönen Ausflug nach Freiburg. Stadtführung mit dem Weltberühmten Dominik Heiny. Kinpen Tour vom Feinsten",
                Departure = "Freiburg HBF",
                StartDate = DateTime.Parse("13/03/2020"),
                EndDate = DateTime.Parse("15/03/2020"),
                MaxTraveler = 20,
                TravelId = "2389e23bu7r2893",
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageID="picture1",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(45).jpg",
                      
                    },
                    new Image
                    {
                        ImageID="picture2",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(46).jpg",
                        
                    },
                     new Image
                    {
                        ImageID="picture3",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(47).jpg",
                        
                    }
                },


            };

            Travel t2 = new Travel
            {
                Destination = "Freiburg",
                Costs = 20.00,
                Description = "Wünderschönen Ausflug nach Freiburg. Stadtführung mit dem Weltberühmten Dominik Heiny. Kinpen Tour vom Feinsten",
                Departure = "Freiburg HBF",
                StartDate = DateTime.Parse("13/03/2020"),
                EndDate = DateTime.Parse("15/03/2020"),
                MaxTraveler = 20,
                TravelId = "2389e23bu7r2893",
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageID="picture1",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(45).jpg",
                        
                    },
                    new Image
                    {
                        ImageID="picture2",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(46).jpg",
                       
                    },
                     new Image
                    {
                        ImageID="picture3",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(47).jpg",
                       
                    }
                },


            };
            Travel t3 = new Travel
            {
                Destination = "Freiburg",
                Costs = 20.00,
                Description = "Wünderschönen Ausflug nach Freiburg. Stadtführung mit dem Weltberühmten Dominik Heiny. Kinpen Tour vom Feinsten",
                Departure = "Freiburg HBF",
                StartDate = DateTime.Parse("13/03/2020"),
                EndDate = DateTime.Parse("15/03/2020"),
                MaxTraveler = 20,
                TravelId = "2389e23bu7r2893",
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageID="picture1",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(45).jpg",
                        
                    },
                    new Image
                    {
                        ImageID="picture2",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(46).jpg",
                        
                    },
                     new Image
                    {
                        ImageID="picture3",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(47).jpg",
                        
                    }
                },


            };
            Travel t4 = new Travel
            {
                Destination = "Freiburg",
                Costs = 20.00,
                Description = "Wünderschönen Ausflug nach Freiburg. Stadtführung mit dem Weltberühmten Dominik Heiny. Kinpen Tour vom Feinsten",
                Departure = "Freiburg HBF",
                StartDate = DateTime.Parse("13/03/2020"),
                EndDate = DateTime.Parse("15/03/2020"),
                MaxTraveler = 20,
                TravelId = "2389e23bu7r2893",
                Images = new List<Image>
                {
                    new Image
                    {
                        ImageID="picture1",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(45).jpg",
                       
                    },
                    new Image
                    {
                        ImageID="picture2",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(46).jpg",
                       
                    },
                     new Image
                    {
                        ImageID="picture3",
                        ImageUrl="https://mdbootstrap.com/img/Photos/Slides/img%20(47).jpg",
                       
                    }
                },
            };

            List<Travel> t_List = new List<Travel>();
            t_List.Add(t1);
            t_List.Add(t2);
            t_List.Add(t3);
            t_List.Add(t4);
            */
            List<Travel> past = _parteiDbContext.Travels.Include(x => x.Images).Where(x=>x.StartDate < DateTime.Today).OrderBy(x => x.StartDate).ToList();
            List<Travel>  upcoming = _parteiDbContext.Travels.Include(x => x.Images).Where(x => x.StartDate >= DateTime.Today).OrderBy(x => x.StartDate).ToList();

            List<Travel> travelList = _parteiDbContext.Travels.ToList();
            var maxTravel = _parteiDbContext.Travels.FirstOrDefault().MaxTraveler;
            Dictionary<int, double> id_percent = new Dictionary<int, double>();
            double percent = 0;
            foreach (Travel travel in travelList)
            {
                var actualCosts = _parteiDbContext.TravelMembers.Where(x=>x.Travel.TravelId.Equals(travel.TravelId)).Sum(x=>x.ActualCosts);
                var targetCosts = _parteiDbContext.TravelMembers.Where(x => x.Travel.TravelId.Equals(travel.TravelId)).Select(x => x.TargetCosts).FirstOrDefault();
                if (targetCosts > 0)
                {
                    percent = (100.0 / (targetCosts * maxTravel)) * actualCosts;
                }
                id_percent.Add(travel.TravelId, percent);
                percent = 0;
            }
            
            TripOverviewViewModel tripOverviewViewModel = new TripOverviewViewModel()
            {
                PastTravels = new Tuple<List<Travel>, Dictionary<int, double>>(past,id_percent),
                UpcomingTravels = new Tuple<List<Travel>, Dictionary<int, double>>(upcoming, id_percent),
                
            };


            return View(tripOverviewViewModel);
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