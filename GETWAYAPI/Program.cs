using Microsoft.AspNetCore.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddReverseProxy()
                  .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));
builder.Services.AddRateLimiter(rateLimiterOptions =>
{
    rateLimiterOptions.AddFixedWindowLimiter("bookingFixed", option =>
    {
        option.Window = TimeSpan.FromSeconds(5);
        option.PermitLimit = 2;
    });
});
//builder.Services.AddCors(otp =>
//{
//    otp.AddPolicy("onlinecarering", builder =>
//    {
//        builder.WithOrigins("http://localhost:3000")
//        .AllowAnyMethod()
//        .AllowAnyHeader()
//        .AllowCredentials();
//    });
//});

var app = builder.Build();
app.UseCors(builder => builder
     .AllowAnyOrigin()
     .AllowAnyMethod()
     .AllowAnyHeader());

app.UseRateLimiter();
app.MapReverseProxy(); ;
app.UseCors();

app.Run();