using Microsoft.Extensions.DependencyInjection;

namespace Shortener.GrpcServices;

public static class DependencyInjections
{
    public static IServiceCollection AddShortenGrpcClient(this IServiceCollection services, string url)
    {
        services.AddGrpcClient<ShortenUrl.ShortenUrlClient>(options => {
            options.Address = new Uri(url);
        });

        return services;
    }
}
