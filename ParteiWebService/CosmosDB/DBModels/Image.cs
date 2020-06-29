using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Image
    {
        [BsonElement("ImageId")]
        public Guid ImageId { get; set; } // Shard Key

        [BsonElement("imagename")]
        public string ImageName { get; set; }

        [BsonElement("bloburl")]
        public string BlobUrl { get; set; }

        public Image()
        {
            ImageId = Guid.NewGuid();
        }
    }
}
