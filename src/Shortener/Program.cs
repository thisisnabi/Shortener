using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using Serilog.Sinks.SystemConsole.Themes;
using Shortener.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();


builder.Logging.ClearProviders();

builder.Host.UseSerilog((webHostBuilderContext, loggerConfiguration) =>
{
    loggerConfiguration.ReadFrom.Configuration(webHostBuilderContext.Configuration)
        .Enrich.FromLogContext()
        .Enrich.WithProperty("service_name", "Nabi");

    loggerConfiguration.WriteTo.Async(loggerSinkConfiguration =>
    {
        loggerSinkConfiguration.Console(LogEventLevel.Debug,
            theme: AnsiConsoleTheme.Code,
            outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3} <{SourceContext}>] {Message:lj} {Properties:j}{NewLine}{Exception}");
 

        loggerSinkConfiguration.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://127.0.0.1:9200"))
        {
            AutoRegisterTemplate = false,
            ConnectionTimeout = TimeSpan.FromSeconds(30),
            InlineFields = true,
            MinimumLogEventLevel = LogEventLevel.Information,
            IndexDecider = (logEvent, dateTimeOffset) =>
                $"shortener-{dateTimeOffset:yyyy-MM-dd}-{logEvent.Level.ToString().ToLowerInvariant()}",
            EmitEventFailure = EmitEventFailureHandling.WriteToSelfLog |
                               EmitEventFailureHandling.RaiseCallback,
            FailureCallback = logEvent => Console.WriteLine(logEvent.Exception!.ToString())
        });
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMemoryCache();

builder.Services.AddScoped<ShortenUrlService>();
builder.Services.AddSingleton<ShortenDiagnostic>();

var settings = builder.Configuration.Get<AppSettings>();
builder.Services.Configure<AppSettings>(builder.Configuration);

builder.Services.AddDbContext<ShortenerDbContext>(options =>
{
    if (settings is null)
    {
        // TODO: create custom excetion
        throw new Exception("Invalid settings!");
    }
    options.UseMongoDB(settings.MongoDbSetting.Host,
                       settings.MongoDbSetting.DatabaseName);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
 
app.MapShortenEndpoint();
app.MapRedirectEndpoint();
 
 
app.Run();


