using Microsoft.Extensions.Options;
using Shortener.Models;
using Shortener.Persistence;

namespace Shortener.Services;

public class ShortenUrlService
{
    private readonly ShortenerDbContext _dbContext;
    private readonly AppSettings _appSettings;
    public ShortenUrlService(
        ShortenerDbContext dbContext,
        IOptions<AppSettings> options)
    {
        _dbContext = dbContext;
        _appSettings = options.Value;
    }
     
    public async Task<string> GetShortenUrl(string destinationUrl, CancellationToken cancellation)
    {
        var shortenCode = GenerateCode();

        var link = new Link
        {
            CreatedOn = DateTime.UtcNow,
            DestinationUrl = destinationUrl,
            ShortenUrl = shortenCode
        };
         
        await _dbContext.Links.AddAsync(link, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return $"{_appSettings.BaseUrl}/{shortenCode}";
    } 
    private static string GenerateCode()
    {
        return Guid.NewGuid().ToString().Substring(6);
    }
}
