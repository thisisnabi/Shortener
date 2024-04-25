using Microsoft.Extensions.Options;
using Shortener.Models;
using Shortener.Persistence;
using System.Security.Cryptography;
using System.Text;

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
        var shortenCode = GenerateCode(destinationUrl);

        var link = new Link
        {
            CreatedOn = DateTime.UtcNow,
            DestinationUrl = destinationUrl,
            ShortenCode = shortenCode
        };

        await _dbContext.Links.AddAsync(link, cancellation);
        await _dbContext.SaveChangesAsync(cancellation);

        return $"{_appSettings.BaseUrl}/{shortenCode}";
    }

    public string GenerateCode(string longUrl)
    {
        using MD5 md5 = MD5.Create();

        byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
        string hashCode = BitConverter.ToString(hashBytes)
                                      .Replace(oldValue: "-", newValue: "")
                                      .ToLower();

        for (int i = 0; i <= hashCode.Length - _appSettings.ShortCodeLength; i++)
        {
            string candidateCode = hashCode.Substring(i, _appSettings.ShortCodeLength);
            // TODO: duplicated check 

            return candidateCode;
        }

        // TODO: Use custom exception
        throw new Exception(Constants.Exceptions.FailedGenerateUniqueCode);
    }
}
