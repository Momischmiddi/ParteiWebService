using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class TravelMember
    {
        [BsonElement("TravelMemberId")]
        public Guid TravelMemberId { get; set; } // Shard Key

        [BsonElement("Targetcosts")]
        public double TargetCosts { get; set; }

        [BsonElement("Actualcosts")]
        public double ActualCosts { get; set; }

        [BsonElement("Travelid")]
        public int TravelId { get; set; }

        [BsonElement("Stopid")]
        public Guid StopId { get; set; }

        public TravelMember()
        {
            TravelMemberId = Guid.NewGuid();
        }
    }
}
