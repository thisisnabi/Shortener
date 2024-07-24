using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Shortener.Diagnostics;
using Shortener.Models;
using System.Security.Cryptography;
using System.Text;

namespace Shortener.Services;

public class ShortenUrlService
{
    private readonly ShortenerDbContext _dbContext;
    private readonly AppSettings _appSettings;
    private readonly IMemoryCache _memoryCache;

    private readonly ShortenDiagnostic _shortenDiagnostic;

    public ShortenUrlService(
        ShortenerDbContext dbContext,
        IMemoryCache memoryCache,
        IOptions<AppSettings> options,
        ShortenDiagnostic shortenDiagnostic)
    {
        _dbContext = dbContext;
        _appSettings = options.Value;
        _memoryCache = memoryCache;
        _shortenDiagnostic = shortenDiagnostic;
    }

    public async Task<string> GenerateShortenUrlAsync(string destinationUrl, CancellationToken cancellation)
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

    internal async Task<string> GetDestinationUrlAsync(string shortCode, CancellationToken cancellationToken)
    {
        if (_memoryCache.TryGetValue(shortCode, out string destinationUrl))
        {
            _shortenDiagnostic.AddRedirection(shortCode);
            return destinationUrl;
        }

        var link = await _dbContext.Links.FirstOrDefaultAsync(x => x.ShortenCode == shortCode, cancellationToken);

        if (link is not null)
        {
            _memoryCache.Set(shortCode, link.DestinationUrl);
            _shortenDiagnostic.AddRedirection(shortCode);
            return link.DestinationUrl;
        }

        _shortenDiagnostic.AddFailedRedirection();
        // TODO: Please decluare a custom exception
        throw new Exception("Invalid shorten code!");
    }
}
