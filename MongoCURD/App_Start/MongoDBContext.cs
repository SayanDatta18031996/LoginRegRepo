using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MongoDB.Driver;
using System.Configuration;

namespace MongoCURD.App_Start
{
    public class MongoDBContext
    {
        MongoClient client;
        public IMongoDatabase database;
        public MongoDBContext()
        {
            var mongoClient = new MongoClient(ConfigurationManager.AppSettings["MongoDBHost"]);
            database = mongoClient.GetDatabase(ConfigurationManager.AppSettings["MongoDatabaseName"]);
        }
    }
}