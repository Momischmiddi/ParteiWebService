using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace DataAccessLibrary.Models
{
    public class ExternalTravelMember
    {
        [Key]
        public int ID { get; set; }
        [Display(Name = "Soll Kosten")]
        public double TargetCosts { get; set; }
        [Display(Name = "Ist Kosten")]
        public double ActualCosts { get; set; }
        public Travel Travel { get; set; }
        public ExternalMember ExternalMember { get; set; }


        public Stop Stop { get; set; }
        public int? StopId { get; set; }
    }
}
