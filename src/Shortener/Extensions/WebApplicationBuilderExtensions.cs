using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;

namespace Shortener.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static void ConfigureObservability(this WebApplicationBuilder builder)
    {
        builder.Services.AddOpenTelemetry()
                .ConfigureResource(builder => builder.AddService(serviceName: "ShortenService",
                                                                 serviceVersion: "v1.0.1"))
                .WithMetrics(builder =>
                {
                    builder.AddPrometheusExporter();
                    var meters = new string[] { ShortenDiagnostic.MeterName };
                    builder.AddMeter(meters);
                });

        builder.Services.AddSingleton<ShortenDiagnostic>();
    }

    public static void ConfigureAppSettings(this WebApplicationBuilder builder)
    {
        builder.Services.Configure<AppSettings>(builder.Configuration);
    }

    public static void ConfigureDbContext(this WebApplicationBuilder builder)
    {
        var settings = builder.Configuration.Get<AppSettings>();
        builder.Services.AddDbContext<ShortenerDbContext>(options =>
        {
            if (settings is null)
                throw new ArgumentNullException(nameof(settings));

            options.UseMongoDB(settings.MongoDbSetting.Host,
                               settings.MongoDbSetting.DatabaseName);
        });
    }
}
