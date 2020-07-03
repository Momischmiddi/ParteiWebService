using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class TravelStop
    {
        [BsonElement("TravelStopId")]
        public Guid TravelStopId { get; set; } // Shard Key

        [BsonElement("TravelId")]
        public Guid TravelId { get; set; }

        [BsonElement("StopId")]
        public Guid StopId { get; set; }

        public TravelStop()
        {
            TravelStopId = Guid.NewGuid();
        }
    }
}
