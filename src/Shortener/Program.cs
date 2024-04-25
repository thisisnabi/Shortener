using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shortener;
using Shortener.Contracts;
using Shortener.Persistence;
using Shortener.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ShortenUrlService>();

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

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapPost("/shorten", async (
    [FromBody] ShortenRequest request,
    ShortenUrlService shortenService,
    CancellationToken cancellationToken
    ) =>
{
    return await shortenService.GetShortenUrl(request.Url, cancellationToken);
});











app.Run();


