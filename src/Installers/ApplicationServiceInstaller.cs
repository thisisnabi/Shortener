namespace Devblogs.Services.Shortener.Installers;

public sealed class ApplicationServiceInstaller : IServiceCollectionInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IUrlShortenerService, UrlShortenerService>();
        services.AddScoped<IShortCodeHandler, ShortCodeHandler>();
    }
}
