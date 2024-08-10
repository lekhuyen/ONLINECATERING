using INFORMATION.API.Models;
using MongoDB.Driver;

namespace INFORMATIONAPI.Models
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _mongoDB;
        public DatabaseContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDB"));
            _mongoDB = client.GetDatabase("INFORMATIONMODULE");
        }
        public IMongoCollection<About> About => _mongoDB.GetCollection<About>("About");

        public IMongoCollection<AboutType> AboutType => _mongoDB.GetCollection<AboutType>("AboutType");


        public IMongoCollection<News> News => _mongoDB.GetCollection<News>("News");

        public IMongoCollection<NewsType> NewsType => _mongoDB.GetCollection<NewsType>("NewsType");

        public IMongoCollection<Contact> Contact => _mongoDB.GetCollection<Contact>("Contact");
        
        public IMongoCollection<Subscription> Subscriptions => _mongoDB.GetCollection<Subscription>("Subscriptions");

        public async Task<bool> IsEmailSubscribedAsync(string email)
        {
            var filter = Builders<Subscription>.Filter.Eq(s => s.Email, email);
            var count = await Subscriptions.CountDocumentsAsync(filter);
            return count > 0;
        }
    }
}
