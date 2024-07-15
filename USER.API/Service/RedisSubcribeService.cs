
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using System.Net.NetworkInformation;
using USER.API.Models;

namespace USER.API.Service
{
    public class RedisSubcribeService : IHostedService
    {
        private readonly RedisClient _redisClient;
        private readonly DatabaseContext _dbContext;
        public RedisSubcribeService(RedisClient redisClient, DatabaseContext dbContext)
        {
            _redisClient = redisClient;
            _dbContext = dbContext;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _redisClient.Subcribe("create-booking", handleBooking);
            _redisClient.Subcribe("edit-booking", handleEditBooking);
            return Task.CompletedTask;
        }
        public async void handleBooking(string message)
        {
            var booking = JsonConvert.DeserializeObject<Booking>(message);
            var bookingInfo = new Booking
            {
                UserId = booking.UserId,
                RestaurantId = booking.RestaurantId,
                MenuId = booking.MenuId,
                DayArrive = booking.DayArrive,
                Hour = booking.Hour,
                Status = booking.Status,
                Member = booking.Member,
                Pont = booking.Pont,
                Total = booking.Total,
                Description = booking.Description,
            };
            await _dbContext.UserBooking.AddAsync(bookingInfo);
            await _dbContext.SaveChangesAsync();
        }
        public async void handleEditBooking(string message)
        {
            var booking = JsonConvert.DeserializeObject<Booking>(message);
            var bookingEdit = await _dbContext.UserBooking
                .FirstOrDefaultAsync(b => b.UserId == booking.UserId && b.RestaurantId == booking.RestaurantId);
            if (bookingEdit != null)
            {
                bookingEdit.MenuId = bookingEdit.MenuId;
                bookingEdit.DayArrive = booking.DayArrive;
                bookingEdit.Hour = booking.Hour;
                bookingEdit.Status = booking.Status;
                bookingEdit.Member = booking.Member;
                bookingEdit.Pont = booking.Pont;
                bookingEdit.Total = booking.Total;
                bookingEdit.Description = booking.Description;
            }
            
             _dbContext.UserBooking.Update(bookingEdit);
            await _dbContext.SaveChangesAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
