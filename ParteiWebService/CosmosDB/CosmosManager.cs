using System;
using System.Security.Authentication;
using Aufgabe_2.CosmosDBModels;
using MongoDB.Driver;

namespace Aufgabe_2
{
    public class CosmosManager
    {
        public static IMongoCollection<Image> images;
        public static IMongoCollection<Stop> stops;
        public static IMongoCollection<TravelMember> travelMembers;
        public static IMongoCollection<Travel> travels;

        public static void Setup()
        {
            try {
                MongoClientSettings settings = new MongoClientSettings();
                settings.Server = new MongoServerAddress("cloudbobmongo.mongo.cosmos.azure.com", 10255);
                settings.UseSsl = true;
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
                settings.RetryWrites = false;

                MongoIdentity identity = new MongoInternalIdentity("cloudbobdb", "cloudbobmongo");
                MongoIdentityEvidence evidence = new PasswordEvidence(Credentials.CosmosPasswordEvidence);

                settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

                MongoClient client = new MongoClient(settings);

                var mongoDataBase = client.GetDatabase("cloudbobdb");

                images = mongoDataBase.GetCollection<Image>("images");
                stops = mongoDataBase.GetCollection<Stop>("stops");
                travelMembers = mongoDataBase.GetCollection<TravelMember>("travelmembers");
                travels = mongoDataBase.GetCollection<Travel>("travels");
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
