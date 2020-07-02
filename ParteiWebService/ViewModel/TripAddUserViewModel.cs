using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aufgabe_2.ViewModel
{
    public class TripAddUserViewModel
    {

        public List<Member> Members { get; set; }
        public List<ExternalMember> ExternalMembers { get; set; }
        public IEnumerable<string> SelectedExternalMemeberIDs { get; set; }
        public List<TravelMember> TravelMembers { get; set; }
        public Travel Travel { get; set; }
        public List<Stop> Stops { get; set; }
        public List<ExternalTravelMember> ExternalTravelMembers { get; set; }

    }
}