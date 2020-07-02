using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DataAccessLibrary.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Member> Members { get; } = new List<Member>();
        public ApplicationUser Admin { get; set; }
        public string AdminId { get; set; }
        public string OrganizationImage { get; set; }
        public IList<Travel> Travels{ get; set; }
    }
}
