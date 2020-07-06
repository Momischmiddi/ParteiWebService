using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ParteiWebService.CosmosDB.DBModels
{
    public class Organization
    {
        [BsonElement("OrganizationId")]
        public Guid OrganizationId { get; set; } // Shard Key

        [BsonElement("Id")]
        public int Id { get; set; }

        [BsonElement("Name")]
        public string Name { get; set; }

        [BsonElement("AdminId")]
        public string AdminId { get; set; }

        [BsonElement("OrganizationImageUrl")]
        public string OrganizationImageUrl { get; set; }

        public Organization()
        {
            OrganizationId = Guid.NewGuid();
        }
    }
}
