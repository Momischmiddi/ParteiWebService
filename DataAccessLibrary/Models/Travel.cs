using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataAccessLibrary.Models
{
    public class Travel
    {
        [Key]
        public int TravelId { get; set; }
        [Display(Name = "Reiseziel")]
        public string Destination { get; set; }
        [Display(Name = "Reisestart"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        [Display(Name = "Reiseende"), DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        [Display(Name = "Reisebeschreibung")]
        public string Description { get; set; }
        [Display(Name = "Abfahrtsort")]
        public string Departure { get; set; }
        [Display(Name = "Kosten")]
        public double Costs { get; set; }
        [Display(Name = "Maximale Teilnehmeranzahl")]
        public int MaxTraveler { get; set; }

        public List<Image> Images { get; set; }

        public List<ExternalMember> ExternalMembers { get; } = new List<ExternalMember>();

        public List<TravelMember> TravelMembers { get; } = new List<TravelMember>();
    }
}