using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataAccessLibrary.Models
{
    namespace DataAccessLibrary.Models
    {
        public class Stop
        {
            [Key]
            public int StopId { get; set; }
            [Display(Name = "StopName")]
            public string StopName { get; set; }          
        }
    }
}
