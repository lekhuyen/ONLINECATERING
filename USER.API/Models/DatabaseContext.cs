using MongoDB.Driver;

namespace USER.API.Models
{
    public class DatabaseContext
    {
        private readonly IMongoDatabase _mongoDatabase;
        public DatabaseContext(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("MongoDb"));

            _mongoDatabase = client.GetDatabase("OnlineCatering");
        }
        public IMongoCollection<User> Users => _mongoDatabase.GetCollection<User>("Users");
        // Hello
    }
}
