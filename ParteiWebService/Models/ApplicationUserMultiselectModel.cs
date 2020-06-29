﻿using DataAccessLibrary.Models;
using System.Collections.Generic;

namespace ParteiWebService.Models
{
    public class ApplicationUserMultiselectModel
    {
        public IEnumerable<ApplicationUser> ApplicationUsers { get; set; }
        public IEnumerable<string> SelectedMemberIDs { get; set; }
    }
}
