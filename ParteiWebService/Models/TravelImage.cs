using Microsoft.AspNetCore.Http;

namespace ParteiWebService.Models
{
    public class TravelImage
    {
        public string FileName { get; set; }

        public IFormFile File { get; set; }
    }
}
