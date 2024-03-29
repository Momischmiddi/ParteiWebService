﻿using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.Models
{
    public class ApplicationUserMultiselectModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public IEnumerable<string> SelectedMemberIDs { get; set; }
    }
}
