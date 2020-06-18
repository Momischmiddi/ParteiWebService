using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccessLibrary.DataAccess;
using DataAccessLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Aufgabe_2.Controllers
{
    public class TripOverviewController : Controller
    {

        private readonly BobContext _bobContext;


        public TripOverviewController(BobContext bobContext)
        {
            _bobContext = bobContext;
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
            var allTravels = _bobContext.Travels.Include(x => x.Images).OrderBy(x => x.StartDate).ToList();

            return View(allTravels);
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

  

    }
}