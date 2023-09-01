namespace Devblogs.Shortener.Interfaces;

public interface IUrlShortenerService
{
    Task<(bool found, string? value)> TryGetLongUrlAsync(string shortCode, CancellationToken cancellationToken);
    Task<string> ShortenUrlAsync(string longUrl, CancellationToken cancellationToken);

}
