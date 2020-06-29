using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{

    public class Stop
    {
        [Key]
        public int StopId { get; set; }
        [Display(Name = "Zusteigepunkt")]
        public string StopName { get; set; }

        public ICollection<TravelStop> TravelStops { get; set; }

    }

}
