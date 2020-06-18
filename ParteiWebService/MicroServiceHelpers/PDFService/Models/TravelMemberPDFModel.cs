using System;

namespace CloudbobsPDFRendering.PDFCreators.Trip
{
    /**
     * DO NEVER MODIFY. THE MICRO-SERVICE EXPECTS THOSE MODELS
     */
    public class TravelMemberPDFModel
    {
        public String PreName { get; set; }
        public String LastName { get; set; }
        public String City { get; set; }
        public String Stop { get; set; }
        public double TargetCosts { get; set; }
        public double ActualCosts { get; set; }
    }
}
