using System;
using System.Collections.Generic;

namespace ParteiWebService.ViewModel
{
    public class TripOverviewViewModel
    {
        public Tuple<List<DataAccessLibrary.Models.Travel>, Dictionary<int, double>> PastTravels { get; set; }
        public Tuple<List<DataAccessLibrary.Models.Travel>, Dictionary<int, double>> UpcomingTravels { get; set; }
    }
}
