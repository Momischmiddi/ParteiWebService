using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;

namespace ParteiWebService.ViewModel
{
    public class TripOverviewViewModel
    {
        public List<Travel> PastTravels { get; set; }
        public List<Travel> UpcomingTravels { get; set; }
    }
}
