
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using REDISCLIENT;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
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

            _redisClient.Subcribe("created_menu", handleMenuCreated);
            _redisClient.Subcribe("update_menu", handleMenuUpdate);

            _redisClient.Subcribe("created_restaurant", handleRestaurantCreated);
            return Task.CompletedTask;
        }
        public async void handleRestaurantCreated(string message)
        {
            var resaturant = JsonConvert.DeserializeObject<Restaurant>(message);
            var resaturantInfo = new Restaurant
            {
                Id = resaturant.Id,
                RestaurantName = resaturant.RestaurantName,
                City = resaturant.City,
                Address = resaturant.Address,
                Open = resaturant.Open,
                Close = resaturant.Close,
                Category = resaturant.Category,
                UserId = resaturant.UserId,
            };

            await _dbContext.Restaurants.AddAsync(resaturantInfo);
            await _dbContext.SaveChangesAsync();
        }

        // chua lam xoa menu
        public async void handleMenuUpdate(string message)
        {
            var menu = JsonConvert.DeserializeObject<Menu>(message);
            var menuUpdate = await _dbContext.Menus.FirstOrDefaultAsync(m => m.Id == menu.Id);
            if(menuUpdate != null)
            {
                menuUpdate.MenuName = menu.MenuName;
                menuUpdate.MenuImage = menu.MenuImage;
                menuUpdate.Ingredient = menu.Ingredient;
                menuUpdate.Quatity = menu.Quatity;
                menuUpdate.RestaurantId = menu.RestaurantId;
                menuUpdate.BookingId = menu.BookingId;
                menuUpdate.Price = menu.Price;
            }
            
             _dbContext.Menus.Update(menuUpdate);
            await _dbContext.SaveChangesAsync();
        }
        public async void handleMenuCreated(string message)
        {
            var menu = JsonConvert.DeserializeObject<Menu>(message);
            var menuInfo = new Menu
            {
                MenuName = menu.MenuName,
                MenuImage = menu.MenuImage,
                Ingredient = menu.Ingredient,
                Quatity = menu.Quatity,
                RestaurantId = menu.RestaurantId,
                BookingId = menu.BookingId,
                Price = menu.Price,
            };
            await _dbContext.Menus.AddAsync(menuInfo);
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
                bookingEdit.MenuId = booking.MenuId;
                bookingEdit.DayArrive = booking.DayArrive;
                bookingEdit.Hour = booking.Hour;
                bookingEdit.Status = booking.Status;
                bookingEdit.Member = booking.Member;
                bookingEdit.Pont = booking.Pont;
                bookingEdit.Total = booking.Total;
                bookingEdit.Description = booking.Description;

                _dbContext.UserBooking.Update(bookingEdit);
                await _dbContext.SaveChangesAsync();
            }
            
            
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
