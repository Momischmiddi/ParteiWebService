using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Aufgabe_2.CosmosDBModels
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
