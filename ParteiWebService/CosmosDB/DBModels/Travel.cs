using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Travel
    {
        [BsonId]
        public ObjectId _id { get; set; }

        [BsonElement("TravelId")]
        public Guid TravelId { get; set; } // Shard Key

        [BsonElement("destination")]
        public String Destination { get; set; }

        [BsonElement("startdate")]
        public DateTime StartDate { get; set; }

        [BsonElement("enddate")]
        public DateTime EndDate { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("departure")]
        public string Departure { get; set; }

        [BsonElement("costs")]
        public double Costs { get; set; }

        [BsonElement("maxtravelers")]
        public int MaxTraveler { get; set; }

        [BsonElement("imageids")]
        public List<string> ImageIds { get; set; }

        public Travel()
        {
            TravelId = Guid.NewGuid();
            ImageIds = new List<string>();
        }
    }
}
