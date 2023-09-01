namespace Devblogs.Services.Shortener.Installers;

public sealed class ApplicationSettingInstaller : IServiceCollectionInstaller
{
    public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<UrlShortenerSetting>(configuration.GetSection(UrlShortenerSetting.SectionName));

    }
}
