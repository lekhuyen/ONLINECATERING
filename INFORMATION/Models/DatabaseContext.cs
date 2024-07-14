using MongoDB.Driver;

namespace INFORMATIONAPI.Models
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _mongoDB;
        public DatabaseContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            _mongoDB = client.GetDatabase("InformationTB");
        }
        public IMongoCollection<About> About => _mongoDB.GetCollection<About>("About");

        public IMongoCollection<News> News => _mongoDB.GetCollection<News>("News");

        public IMongoCollection<NewsType> NewsType => _mongoDB.GetCollection<NewsType>("NewsType");

        public IMongoCollection<Contact> Contact => _mongoDB.GetCollection<Contact>("Contact");
    }
}
