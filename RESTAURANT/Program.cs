
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
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

			builder.Services.AddControllers()
				.AddJsonOptions(options =>
				{
					options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
				});


			builder.Services.AddScoped<IRestaurant, RestaurantRepositories>();
			builder.Services.AddScoped<ICategory, CategoryRepositories>();
			builder.Services.AddScoped<IComment, CommentRepositories>();
			builder.Services.AddScoped<ICommentChild, CommentChildRepositories>();
			builder.Services.AddScoped<IMenu, MenuRespositories>();
			builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
			builder.Services.AddScoped<ILobbyRepository, LobbyRepository>();
			builder.Services.AddScoped<ILobbyImagesRepository, LobbyImagesRepository>();

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

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:3000") // Allow requests from this origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });


            var app = builder.Build();

			app.UseCors(builder => builder
				 .AllowAnyOrigin()
				 .AllowAnyMethod()
				 .AllowAnyHeader());

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}
            // Use CORS middleware
            app.UseCors("AllowSpecificOrigin");

			app.UseStaticFiles(new StaticFileOptions
			{
				FileProvider = new PhysicalFileProvider(
				Path.Combine(Directory.GetCurrentDirectory(), "Uploads")),
				RequestPath = "/Uploads"
			});
			app.UseCors();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
