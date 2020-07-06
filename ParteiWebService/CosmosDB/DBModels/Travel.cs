using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Travel
    {
        [BsonElement("TravelId")]
        public Guid TravelId { get; set; } // Shard Key

        [BsonElement("Destination")]
        public String Destination { get; set; }

        [BsonElement("Startdate")]
        public DateTime StartDate { get; set; }

        [BsonElement("Enddate")]
        public DateTime EndDate { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Departure")]
        public string Departure { get; set; }

        [BsonElement("MaxTravelers")]
        public int MaxTraveler { get; set; }

        public Travel()
        {
            TravelId = Guid.NewGuid();
        }
    }
}
