using DataAccessLibrary.Models;
using ParteiWebService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.ViewModel
{
    public class ManagerViewModel
    {
        public ApplicationUserMultiselectModel ApplicationUserMultiselectModel { get; set; }
        public Organization Organization { get; set; }
    }
}
