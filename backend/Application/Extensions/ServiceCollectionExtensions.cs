using Application.Interfaces;
using Application.Services;
using Application.Mappings;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using System.Reflection;

namespace Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // AutoMapper
        services.AddAutoMapper(typeof(MappingProfile));

        // FluentValidation
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        // Services
        services.AddScoped<IEmpleadoService, EmpleadoService>();
        services.AddScoped<ITiendaService, TiendaService>();
        services.AddScoped<IAuthService, AuthService>();

        return services;
    }
}
