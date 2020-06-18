using Microsoft.AspNetCore.Http;

namespace Aufgabe_2.Models
{
    public class TravelImage
    {
        public string FileName { get; set; }

        public IFormFile File { get; set; }
    }
}
