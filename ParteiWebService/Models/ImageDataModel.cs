using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;
using System;

namespace Aufgabe_2.Models
{
    public class ImageDataModel
    {
        [BsonId(IdGenerator = typeof(CombGuidGenerator))]
        public Guid Id { get; set; }

        [BsonElement("imageurl")]
        public string ImageUrl { get; set; }

        [BsonElement("filename")]
        public string FileName { get; set; }

        [BsonElement("filesize")]
        public int FileSize { get; set; }

        [BsonElement("filetype")]
        public String FileType { get; set; }

        [BsonElement("lastmodified")]
        public String LastModified { get; set; }
    }
}
