﻿using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Aufgabe_2.Models
{
    public class ExternalMemberMultiselectModel
    {
        public IEnumerable<ExternalMember> ExternalMemebers { get; set; }
        public IEnumerable<string> SelectedMemberIDs { get; set; }
    }
}
