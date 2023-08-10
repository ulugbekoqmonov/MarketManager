
using MarketManager.API.Middlewares;
using MarketManager.Application;
using MarketManager.Infrastructure;
using Serilog;

namespace MarketManager.API;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddApplication();
        builder.Services.AddInfrastructure(builder.Configuration);
        builder.Services.AddApi(builder.Configuration);
        builder.Host.UseSerilog();
        var app = builder.Build();
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        var availableStylesheets = new List<string>
        {
        "/swagger-ui/SwaggerDark.css",
        "/swagger-ui/theme-flattop.css",
        "/swagger-ui/theme-outline.css",
        "/swagger-ui/theme-newspaper.css",
        "/swagger-ui/theme-muted.css",
        "/swagger-ui/theme-monokai.css",
        "/swagger-ui/theme-material.css",


         };

        var random = new Random();
        var selectedIndex = random.Next(availableStylesheets.Count);
        var selectedStylesheet = availableStylesheets[selectedIndex];

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "MyAPI");
                c.InjectStylesheet("/swagger-ui/theme-flattop.css");
              
            });


        }

        app.UseStaticFiles();

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseGlobalExceptionMiddleware();

        app.UseAuthentication();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}