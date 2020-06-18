using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccessLibrary.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<ApplicationUserRole> UserRoles { get; set; }
        public string MemberId { get; set; }
        public Member Member { get; set; }
        public int OrgranizationId{ get; set; }
        public Organization Organization { get; set; }
    }

}
