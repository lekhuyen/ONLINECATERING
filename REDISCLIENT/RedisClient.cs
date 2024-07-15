using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REDISCLIENT
{
    public class RedisClient
    {
        private readonly IConnectionMultiplexer _redis;
        public RedisClient(string connectionString)
        {
            _redis = ConnectionMultiplexer.Connect(connectionString);
            Console.WriteLine("Connected to Redis");
        }
        public void Publish(string channel, string message)
        {
            var publish = _redis.GetSubscriber();
            publish.Publish(channel, message);
            Console.WriteLine("Publish to Redis");
        }
        public void Subcribe(string channel, Action<string> handleMessage)
        {
            Console.WriteLine("Subcribe");
            var subcribe = _redis.GetSubscriber();
            subcribe.Subscribe(channel, (ch, message) => handleMessage(message));
        }
        
    }
}
