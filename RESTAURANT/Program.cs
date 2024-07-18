
using Microsoft.EntityFrameworkCore;
using REDISCLIENT;
using RESTAURANT.API.Models;
using RESTAURANT.API.Repositories;
using RESTAURANT.API.Servicer;

namespace RESTAURANT
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            //connect db
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB"));
            });

            

            builder.Services.AddScoped<IRestaurant, RestaurantRepositories>();
            builder.Services.AddScoped<ICategory, CategoryRepositories>();
            builder.Services.AddScoped<IComment, CommentRepositories>();
            builder.Services.AddScoped<ICommentChild, CommentChildRepositories>();
            builder.Services.AddScoped<IMenu, MenuRespositories>();

            builder.Services.AddSingleton<RedisClient>(sp =>

                new RedisClient(builder.Configuration.GetValue<string>("Redis:ConnectionStrings")!)
            );

            builder.Services.AddHostedService<RedisSubcribeService>(sp =>
            {
                var scopeFatory = sp.GetRequiredService<IServiceScopeFactory>();
                var redisClient = sp.GetRequiredService<RedisClient>();
                var dbContext = scopeFatory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
                return new RedisSubcribeService(redisClient, dbContext);
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
