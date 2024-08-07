using MongoDB.Bson;

namespace INFORMATION.API.Models
{
    public class Subscription
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }

    }
}
