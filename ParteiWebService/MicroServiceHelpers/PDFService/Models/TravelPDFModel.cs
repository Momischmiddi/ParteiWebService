using System;
using System.Collections.Generic;

namespace CloudbobsPDFRendering.PDFCreators.Trip
{
    /**
     * DO NEVER MODIFY. THE MICRO-SERVICE EXPECTS THOSE MODELS
     */
    public class TravelPDFModel
    {
        public string Destination { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Description { get; set; }
        public double Costs { get; set; }
        public string ImageBlobURL { get; set; }
        public List<TravelMemberPDFModel> Members { get; set; }

        public TravelPDFModel()
        {
            Members = new List<TravelMemberPDFModel>();
        }
    }
}
