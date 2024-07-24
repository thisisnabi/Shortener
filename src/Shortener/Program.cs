using Shortener.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddEnvironmentVariables();
 
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


