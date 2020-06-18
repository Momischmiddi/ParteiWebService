using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class ExternalMember
    {
        [Key]
        public string ID { get; set; }
        [Display(Name = "Vorname")]
        public string PreName { get; set; }
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
        [Display(Name = "E-Mail Adresse")]
        public string Mail { get; set; }
        [Display(Name = "Soll Kosten")]
        public double TargetCosts { get; set; }
        [Display(Name = "Ist Kosten")]
        public double ActualCosts { get; set; }
        [Display(Name = "Zustiegspunkt")]
        public string BoardingPoint { get; set; }
    }
}
