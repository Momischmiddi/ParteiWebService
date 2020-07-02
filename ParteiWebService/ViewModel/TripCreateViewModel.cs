using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aufgabe_2.ViewModel
{
    public class TripCreateViewModel
    {
        public List<int> SelectedStops { get; set; }
        public List<Stop> StopList { get; set; }
        public Travel Travel { get; set; }
        public Stop Stop { get; set; }

    }
}
