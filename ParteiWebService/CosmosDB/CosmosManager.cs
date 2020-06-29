using System;
using System.Security.Authentication;
using MongoDB.Driver;
using ParteiWebService.CosmosDB.DBModels;

namespace ParteiWebService
{
    public class CosmosManager
    {
        public static IMongoCollection<Image> Images;

        public static void Setup()
        {
            try {
                MongoClientSettings settings = new MongoClientSettings();
                settings.Server = new MongoServerAddress("parteimongostorage.mongo.cosmos.azure.com", 10255);
                settings.UseSsl = true;
                settings.SslSettings = new SslSettings();
                settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;
                settings.RetryWrites = false;

                MongoIdentity identity = new MongoInternalIdentity("parteidb", "parteimongostorage");
                MongoIdentityEvidence evidence = new PasswordEvidence(Credentials.CosmosPasswordEvidence);

                settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

                MongoClient client = new MongoClient(settings);

                var mongoDataBase = client.GetDatabase("parteidb");

                Images = mongoDataBase.GetCollection<Image>("Images");
            } 
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
