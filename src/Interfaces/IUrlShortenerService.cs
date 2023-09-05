namespace Devblogs.Shortener.Interfaces;

public interface IUrlShortenerService
{
    Task<string> ShortenUrlAsync(string longUrl, CancellationToken cancellationToken);
    Task<(bool found, string? value)> TryGetLongUrlAsync(string shortCode, CancellationToken cancellationToken);

}
