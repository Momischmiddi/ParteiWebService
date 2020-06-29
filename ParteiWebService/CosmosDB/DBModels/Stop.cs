using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Stop
    {
        [BsonElement("StopId")]
        public Guid StopId { get; set; } // Shard Key

        [BsonElement("stopname")]
        public string StopName { get; set; }

        public Stop()
        {
            StopId = Guid.NewGuid();
        }
    }
}
