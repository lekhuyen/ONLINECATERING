
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using USER.API.Models;
using USER.API.Repositories;

namespace USER
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

            builder.Services.Configure<DatabaseContext>(
                builder.Configuration.GetSection(nameof(DatabaseContext))    
            );
            builder.Services.AddSingleton<DatabaseContext>();

            //repositories

            builder.Services.AddScoped<IRepositories, UserRepositories>();
            builder.Services.AddScoped<IAuthUser, AuthUserRepositories>();

            //jwt
            var key = Encoding.ASCII.GetBytes(builder.Configuration.GetSection("Jwt")["Key"]);

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


            app.MapControllers();

            app.Run();
        }
    }
}
