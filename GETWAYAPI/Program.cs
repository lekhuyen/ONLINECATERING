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

var app = builder.Build();

app.UseRateLimiter();
app.MapReverseProxy(); ;

app.Run();