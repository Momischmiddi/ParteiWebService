using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Image
    {
        [BsonElement("ImageId")]
        public Guid ImageId { get; set; } // Shard Key

        [BsonElement("ImageName")]
        public string ImageName { get; set; }

        [BsonElement("BlobUrl")]
        public string BlobUrl { get; set; }

        [BsonElement("TravelId")]
        public Guid TravelId { get; set; }

        public Image()
        {
            ImageId = Guid.NewGuid();
        }
    }
}
