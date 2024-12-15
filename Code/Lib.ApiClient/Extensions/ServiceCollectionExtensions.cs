using Lib.ApiClient.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Lib.ApiClient.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection RegisterApiClientServices(this IServiceCollection services)
    {
        services.AddSingleton<HttpClient>();
        services.AddSingleton<IApiClient, HttpApiClient>();

        return services;
    }
}
