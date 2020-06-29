using CloudbobsPDFRendering.PDFCreators;
using CloudbobsPDFRendering.PDFCreators.Trip;
using DataAccessLibrary.Models;
using System.Collections.Generic;
using System.Linq;

namespace ParteiWebService.MicroServiceHelpers.PDFService
{
    public class ModelCreators
    {
        public static TravelPDFModel CreateTripPDFModel(Travel travel)
        {
            var members = new List<TravelMemberPDFModel>();

            var imageUrl = "https://cloudbobstorage.blob.core.windows.net/images/freiburg.jfif";

            if(travel.Images.FirstOrDefault() != null)
            {
                imageUrl = travel.Images.FirstOrDefault().ImageUrl;
            }

           /* foreach (ExternalMember externalMember in travel.ExternalMembers)
            {
                members.Add(
                    new TravelMemberPDFModel
                    {
                        ActualCosts = externalMember.ActualCosts,
                        City = "EXTERN",
                        LastName = externalMember.LastName,
                        PreName = externalMember.PreName,
                        Stop = externalMember.BoardingPoint,
                        TargetCosts = externalMember.TargetCosts
                    });
            }*/

            foreach(TravelMember member in travel.TravelMembers)
            {
                members.Add(
                    new TravelMemberPDFModel
                    {
                        ActualCosts = member.ActualCosts,
                        City = member.Member.Home,
                        LastName = member.Member.LastName,
                        PreName = member.Member.PreName,
                        Stop = member.Stop.StopName,
                        TargetCosts = member.TargetCosts
                    });
            }

            return new TravelPDFModel
            {
                Costs = travel.Costs,
                Description = travel.Description,
                Destination = travel.Destination,
                EndDate = travel.EndDate,
                StartDate = travel.StartDate,
                ImageBlobURL = imageUrl,
                Members = members
            };
        }
        public static MemberListPDFModel CreateMemberListPDFModel(List<Member> members)
        {
            var result = new MemberListPDFModel();
            result.Members = new List<MemberListMemberPDFModel>();

            var x = new MemberListMemberPDFModel();

            foreach(Member member in members)
            {
                result.Members.Add(new MemberListMemberPDFModel
                {
                    Address = member.Adress,
                    City = member.Home,
                    Contribution = member.Contribution,
                    DateOfBirth = member.DateOfBirth,
                    LastName = member.LastName,
                    Postal = member.PostCode,
                    PreName = member.PreName
                });
            }

            return result;
        }
    }
}
