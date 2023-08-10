using FluentValidation;
using MarketManager.Application.Common;
using MarketManager.Application.Common.Behaviours;
using MarketManager.Application.Common.JWT.Interfaces;
using MarketManager.Application.Common.JWT.Service;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace MarketManager.Application;
public static class ConfigureServices
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddMediatR(option =>
        {
            option.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            option.AddBehavior(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            option.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));

        });
        services.AddScoped<IUserRefreshToken, RefreshToken>();
        services.AddScoped<IJwtToken, JwtToken>();
        services.AddScoped<GenericExcelReport>();

        return services;
    }
}
