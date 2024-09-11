const string shorten_bad_request = "The URL query string is required and needs to be well formed";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseOrleans(static siloBuilder =>
{
    siloBuilder.UseLocalhostClustering();
    siloBuilder.AddMemoryGrainStorage("urls");
});
 
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/shorten",
    static async (string url, IGrainFactory grains, IConfiguration configuration) =>
    {
        if (string.IsNullOrWhiteSpace(url) || Uri.IsWellFormedUriString(url, UriKind.Absolute) is false)
        {
            return Results.BadRequest(shorten_bad_request);
        }

        var shortCode = GenerateCode(url);

        var shortenerGrain = grains.GetGrain<IUrlShortenerGrain>(shortCode);
        await shortenerGrain.SetLongUrl(url);
         
        var resultBuilder = new UriBuilder(configuration["BaseUrl"]!) { Path = $"/{shortCode}" };
        return Results.Ok(resultBuilder.Uri);
});

app.MapGet("/{short_code:required}",
    static async (IGrainFactory grains, [FromRoute(Name = "short_code")] string shortCode) =>
    {
        var shortenerGrain = grains.GetGrain<IUrlShortenerGrain>(shortCode);

        var url = await shortenerGrain.GetLongUrl();
        var redirectBuilder = new UriBuilder(url);
        return Results.Redirect(redirectBuilder.Uri.ToString());
    });
  
app.Run();


static string GenerateCode(string longUrl)
{
    using MD5 md5 = MD5.Create();
    var hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
    var hashCode = BitConverter.ToString(hashBytes)
                               .Replace(oldValue: "-", newValue: "")
                               .ToLower();

    return hashCode.Substring(10);
}
