using System;

namespace CloudbobsPDFRendering.PDFCreators.Trip
{
    /**
     * DO NEVER MODIFY. THE MICRO-SERVICE EXPECTS THOSE MODELS
     */
    public class MemberListMemberPDFModel
    {
        public String PreName { get; set; }
        public String LastName { get; set; }
        public String Postal { get; set; }
        public String City { get; set; }
        public String Address { get; set; }
        public DateTime DateOfBirth { get; set; }
        public double Contribution { get; set; }
    }
}
