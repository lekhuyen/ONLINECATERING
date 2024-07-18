
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using RESTAURANT.API.Models;
using System.Numerics;

namespace RESTAURANT.API.Servicer
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

            _redisClient.Subcribe("user_created", handleCreadteUser);
            _redisClient.Subcribe("user_update", handleEditUser);
            return Task.CompletedTask;
        }
        public async void handleCreadteUser(string message)
        {
            var user = JsonConvert.DeserializeObject<User>(message);

            var userInfo = new User
            {
                UserEmail = user.UserEmail,
                UserName = user.UserName,
                Phone = user.Phone,
            };
            await _dbContext.User.AddAsync(userInfo);
            await _dbContext.SaveChangesAsync();
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
            await _dbContext.Booking.AddAsync(bookingInfo);
            await _dbContext.SaveChangesAsync();
        }
        public async void handleEditUser(string message)
        {
            var user = JsonConvert.DeserializeObject<User>(message);
            var userEdit = await _dbContext.User
                .FirstOrDefaultAsync(b => b.Id == user.Id);
            if (userEdit != null)
            {
                userEdit.UserName = user.UserName;
                userEdit.UserEmail = user.UserEmail;
                userEdit.Phone = user.Phone;
            }

            _dbContext.User.Update(userEdit);
            await _dbContext.SaveChangesAsync();
        }
        public async void handleEditBooking(string message)
        {
            var booking = JsonConvert.DeserializeObject<Booking>(message);
            var bookingEdit = await _dbContext.Booking
                .FirstOrDefaultAsync(b => b.UserId == booking.UserId && b.RestaurantId == booking.RestaurantId);
            if (bookingEdit != null)
            {
                bookingEdit.MenuId = booking.MenuId;
                bookingEdit.DayArrive = booking.DayArrive;
                bookingEdit.Hour = booking.Hour;
                bookingEdit.Status = booking.Status;
                bookingEdit.Member = booking.Member;
                bookingEdit.Pont = booking.Pont;
                bookingEdit.Total = booking.Total;
                bookingEdit.Description = booking.Description;
            }

            _dbContext.Booking.Update(bookingEdit);
            await _dbContext.SaveChangesAsync();
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
