var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.InstallFromAssembly<IShortenerAssemblyMarker>(builder.Configuration);
    builder.Services.AddMemoryCache();
}

var app = builder.Build();
{
    app.UseDeveloperExceptionPage();

    app.MapPost("/shorten", async ([FromBody] ShortenRequest request,
        IUrlShortenerService urlShortenerService,
        CancellationToken cancellationToken) =>
    {
        var shortUrl = await urlShortenerService.ShortenUrlAsync(request.Url, cancellationToken);
        return Results.Ok(new { ShortUrl = shortUrl });
    }).AddEndpointFilter<ShortenEndpointFilter>();

    app.MapGet("{shortCode}", async ([FromRoute] string shortCode,
        IUrlShortenerService urlShortenerService, 
        CancellationToken cancellationToken) =>
    {
        var foundUrlResult = await urlShortenerService.TryGetLongUrlAsync(shortCode, cancellationToken);

        if (foundUrlResult.found)
        {
            return Results.Redirect(foundUrlResult.value!);
        }

        return Results.BadRequest(Constants.Data.EndPointFilterMessages.InvalidUrl);
    }).AddEndpointFilter<RedirectEndpointFilter>()
      .AllowAnonymous();
}
app.Run();
