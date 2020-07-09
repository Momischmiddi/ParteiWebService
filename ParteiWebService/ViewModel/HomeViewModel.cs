using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.ViewModel
{
    public class HomeViewModel
    {
        public List<Member> Members { get; set; }
        public int OrganizationId { get; set; }

    }
}
