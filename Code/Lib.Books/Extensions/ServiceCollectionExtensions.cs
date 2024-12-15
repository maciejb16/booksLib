using Lib.Books.Abstractions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lib.Books.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterBookServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        services.Configure<BooksApiOptions>(configuration.GetSection("BooksApiOptions"));

        services.AddScoped<IBooksService, BooksService>();
        services.AddScoped<IBooksOrdersService, BooksOrdersService>();

        return services;
    }

    public static IServiceCollection RegisterBookServices(this IServiceCollection services, Action<BooksApiOptions> config)
    {
        ArgumentNullException.ThrowIfNull(config);

        services.Configure(config);

        services.AddScoped<IBooksService, BooksService>();
        services.AddScoped<IBooksOrdersService, BooksOrdersService>();

        return services;
    }
}
