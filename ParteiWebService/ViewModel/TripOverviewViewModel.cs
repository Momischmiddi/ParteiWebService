using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;

namespace Aufgabe_2.ViewModel
{
    public class TripOverviewViewModel
    {
        public List<Travel> PastTravels { get; set; }
        public List<Travel> UpcomingTravels { get; set; }
    }
}
