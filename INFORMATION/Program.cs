
using INFORMATIONAPI.Models;
using INFORMATIONAPI.Repositories;
using INFORMATIONAPI.Service;
using Microsoft.OpenApi.Models;

namespace INFORMATION
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
			builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
			builder.Services.AddSingleton<IWebHostEnvironment>(builder.Environment);
			builder.Services.AddScoped<DatabaseContext>();
			builder.Services.AddScoped<IAboutRepositories, AboutService>();
			builder.Services.AddScoped<INewsRepositories, NewsService>();

			// Add services to the container.

			builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
				c.EnableAnnotations();
			});

			var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
				app.UseSwaggerUI(c =>
				{
					c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
				});
			}

			app.UseStaticFiles();

			app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
