using DataAccessLibrary.Models.DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class Member
    {       
        [Key]
        public string ID { get; set; }
        [Display(Name = "Vorname")]
        public string PreName { get; set; }
        [Display(Name = "Nachname")]
        public string LastName { get; set; }
        [Display(Name = "Geburtstag")]
        public DateTime DateOfBirth { get; set; }
        [Display(Name = "Addresse")]
        public string Adress { get; set; }
        [Display(Name = "Postleitzahl")]
        public string PostCode { get; set; }
        [Display(Name = "Wohnort")]
        public string Home { get; set; }
        [Display(Name = "Beitrag")]
        public double Contribution { get; set; }
        [Display(Name = "Aktivieren")]
        public bool IsActiveMember { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
        public int OrganizationId  { get; set; }
        public Organization Organization { get; set; }
    }
}
