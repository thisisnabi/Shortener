var builder = WebApplication.CreateBuilder(args);

builder.ConfigureObservability();
builder.ConfigureAppSettings();
builder.ConfigureDbContext();
 
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ShortenUrlService>();

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("urls");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapShortenEndpoint();
app.MapRedirectEndpoint();

app.MapPrometheusScrapingEndpoint();

app.MapGrpcService<ShortenService>();

app.Run();


