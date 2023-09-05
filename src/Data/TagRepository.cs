namespace Devblogs.Shortener.Data;

public class TagRepository : ITagRepository
{
    private readonly ShortenerDbContext _shortenerDbContext;

    public TagRepository(ShortenerDbContext shortenerDbContext)
        => _shortenerDbContext = shortenerDbContext;

    public async Task AddAsync(Tag tag, CancellationToken cancellationToken)
        => await _shortenerDbContext.Tags.AddAsync(tag, cancellationToken);

    public async Task<(bool found, string? value)> TryGetLongUrlAsync(string shortCode, CancellationToken cancellationToken)
    {
        var tag = await _shortenerDbContext.Tags.FirstOrDefaultAsync(x => x.ShortCode == shortCode, cancellationToken);

        if (tag != null)
        {
            return (true, tag.LongUrl);
        }

        return (false, null);
    }

    public async Task<(bool found, string? value)> TryGetShortUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        var tag = await _shortenerDbContext.Tags.FirstOrDefaultAsync(x => x.LongUrl == longUrl, cancellationToken);

        if (tag != null)
        {
            return (true, tag.ShortCode);
        }

        return (false, null);
    }
     
    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken)
        => await _shortenerDbContext.SaveChangesAsync(cancellationToken) > 0;
}
