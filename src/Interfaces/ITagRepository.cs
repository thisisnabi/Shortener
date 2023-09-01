namespace Devblogs.Shortener.Interfaces;

public interface ITagRepository
{
    Task AddAsync(Tag tag, CancellationToken cancellationToken);
    Task<(bool found, string? value)> TryGetLongUrlAsync(string shortCode, CancellationToken cancellationToken);
    Task<(bool found, string? value)> TryGetShortUrlAsync(string longUrl, CancellationToken cancellationToken);
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken);

}
