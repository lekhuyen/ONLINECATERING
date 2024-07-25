
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using REDISCLIENT;
using System.Text;
using USER.API.Models;
using USER.API.Repositories;
using USER.API.Service;

namespace USER
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            ConfigurationManager configuration = builder.Configuration;
            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //email
            builder.Services.Configure<EmailSetting>(builder.Configuration.GetSection("EmailSettings"));
            builder.Services.AddTransient<EmailServices>();


            //connect db
            builder.Services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectDB"));
            });
            
            

            //redis
            builder.Services.AddSingleton<RedisClient>(sp =>
                new RedisClient(builder.Configuration.GetValue<string>("Redis:ConnectionStrings")!));

            builder.Services.AddHostedService<RedisSubcribeService>(sp =>
            {
                var scopeFatory = sp.GetRequiredService<IServiceScopeFactory>();
                var redisClient = sp.GetRequiredService<RedisClient>();
                var dbContext = scopeFatory.CreateScope().ServiceProvider.GetRequiredService<DatabaseContext>();
                return new RedisSubcribeService(redisClient, dbContext);
            });

            //repositories

            // Configure CORS policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder => builder
                        .WithOrigins("http://localhost:3000") // Allow requests from this origin
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            builder.Services.AddScoped<IRepositories, UserRepositories>();
            builder.Services.AddScoped<IAuthUser, AuthUserRepositories>();
            builder.Services.AddScoped<IFavoriteList, FavoriteRespositories>();
            //jwt
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt")["Key"]!);

            builder.Services.AddAuthentication
            (
                x =>
                {
                    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }
            ).AddJwtBearer(y =>
            {
                y.SaveToken = true;
                y.RequireHttpsMetadata = false;
                y.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero,
                };
            });
            

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");


            app.MapControllers();

            app.Run();
        }
    }
}
