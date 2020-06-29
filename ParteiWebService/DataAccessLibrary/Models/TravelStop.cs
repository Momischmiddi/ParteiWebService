using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class TravelStop
    {
        public int TravelId { get; set; }
        public virtual Travel Travel { get; set; }

        public int StopId { get; set; }
        public virtual Stop Stop { get; set; }
    }
}
