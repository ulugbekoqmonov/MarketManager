using System.Text.Json.Serialization;
using MarketManager.API.Common.Services;
using MarketManager.Application.Common.Interfaces;
using MarketManager.Application.Common.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Telegram.Bot;
using TelegramSink;

namespace MarketManager.API;

public static class ConfigureServices
{
    public static IServiceCollection AddApi(this IServiceCollection services, IConfiguration configuration)
    {
        SerilogSettings(configuration);
        //services.AddHostedService<BotBackgroundService>();
        services.AddSingleton<ITelegramBotClient>(
            new TelegramBotClient(configuration?.GetConnectionString("TelegramToken")));
        //services.AddTransient<IUpdateHandler, UpdateHandler>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
        services.AddControllers().AddJsonOptions(x =>
        {
            x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
        
        services.AddEndpointsApiExplorer();
        services.AddAuthorization();
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtSettings(configuration);
        services.AddHttpContextAccessor();
        services.AddSwaggerGen();

        services.AddStackExchangeRedisCache(option =>
        {
            option.Configuration = configuration.GetConnectionString("RedisConnect");
            option.InstanceName = "master";
        });


        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Example API", Version = "v1" });

            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Scheme = "bearer",
                Description = "Please insert JWT token into field"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
            });
            c.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date"
            });

        });
        return services;
    }
    public static void SerilogSettings(IConfiguration configuration)
    {
        Log.Logger = new LoggerConfiguration()
           .ReadFrom.Configuration(configuration)
           .MinimumLevel.Information()
           .WriteTo.Console()
           .Enrich.FromLogContext()
           .Enrich.WithEnvironmentUserName()
           .Enrich.WithMachineName()
           .Enrich.WithClientIp()
           .WriteTo.TeleSink(
            telegramApiKey: configuration.GetConnectionString("TelegramToken"),
            telegramChatId: "-1001856623462",
            minimumLevel: LogEventLevel.Error)
           .CreateLogger();
    }
}
