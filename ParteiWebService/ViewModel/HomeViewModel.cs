using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParteiWebService.ViewModel
{
    public class HomeViewModel
    {
        public IFormFile File { get; set; }
        public int OrganisationId { get; set; }

        public HomeViewModel (IFormFile file, int organisationId)
        {
            this.File = file;
            this.OrganisationId = organisationId;
        }

        public HomeViewModel()
        {

        }
    }
}
