using DataAccessLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.ViewModel
{
    public class AddExternalMemberViewModel
    {
        public ExternalMember ExternalMember { get; set; }

        public Travel Travel { get; set; }

        public AddExternalMemberViewModel(ExternalMember externalMember, Travel travel)
        {
            this.ExternalMember = externalMember;
            this.Travel = travel;
        }
    }
}
