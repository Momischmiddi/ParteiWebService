using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Aufgabe_2.CosmosDBModels
{
    public class TravelMember
    {
        [BsonElement("TravelMemberId")]
        public Guid TravelMemberId { get; set; } // Shard Key

        [BsonElement("targetcosts")]
        public double TargetCosts { get; set; }

        [BsonElement("actualcosts")]
        public double ActualCosts { get; set; }

        [BsonElement("travelid")]
        public Guid TravelId { get; set; }

        [BsonElement("stopid")]
        public Guid StopId { get; set; }

        public TravelMember()
        {
            TravelMemberId = Guid.NewGuid();
        }
    }
}
