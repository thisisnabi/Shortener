namespace Devblogs.Shortener.Services;

public sealed class UrlShortenerService : IUrlShortenerService
{
    private readonly UrlShortenerSetting _shortenerSetting;
    private readonly ITagRepository _linkRepository;
    private readonly IMemoryCache _cache;
    private readonly IShortCodeHandler _shortCodeHandler;

    private Dictionary<string, string> shortToLongUrlMap;

    public UrlShortenerService(
        IOptions<UrlShortenerSetting> shortenerSettingOptions,
        ITagRepository linkRepository,
        IMemoryCache cache,
        IShortCodeHandler shortCodeHandler)
    {
        _shortenerSetting = shortenerSettingOptions.Value;
        shortToLongUrlMap = new Dictionary<string, string>();
        _linkRepository = linkRepository;
        _cache = cache;
        _shortCodeHandler = shortCodeHandler;
    }

    public async Task<string> ShortenUrlAsync(string longUrl, CancellationToken cancellationToken)
    {
        var getUrlResult = await TryGetShortUrlAsync(longUrl, cancellationToken);
        if (getUrlResult.found)
        {
            return UrlResponseCombination(getUrlResult.value!);
        }

        var shortCode = await _shortCodeHandler.GenerateAsync(longUrl, _shortenerSetting.ShortCodeLength);

        var link = Tag.Create(shortCode, longUrl);
        await _linkRepository.AddAsync(link, cancellationToken);
        await _linkRepository.SaveChangesAsync(cancellationToken);

        SetCacheEntry(shortCode, longUrl);
        return UrlResponseCombination(shortCode);
    }

    public async Task<(bool found, string? value)> TryGetLongUrlAsync(string shortCode,
        CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(shortCode, out string? longUrl))
        {
            return (true, longUrl);
        }

        var getTagResult = await _linkRepository.TryGetLongUrlAsync(shortCode, cancellationToken);
        if (getTagResult.found)
        {
            SetCacheEntry(shortCode, getTagResult.value!);
            return (true, getTagResult.value);
        }

        return (false, null);
    }

    public async Task<(bool found, string? value)> TryGetShortUrlAsync(string longUrl,
        CancellationToken cancellationToken)
    {
        if (_cache.TryGetValue(longUrl, out string? shortCode))
        {
            return (true, shortCode);
        }

        var getShortTagResult = await _linkRepository.TryGetShortUrlAsync(longUrl, cancellationToken);
        if (getShortTagResult.found)
        {
            SetCacheEntry(shortCode: getShortTagResult.value!, longUrl: longUrl);
            return (true, getShortTagResult.value);
        }

        return (false, null);
    }

    private void SetCacheEntry(string shortCode, string longUrl)
    {
        _cache.Set(shortCode, longUrl,
            TimeSpan.FromDays(_shortenerSetting.DefaultExpirationCachedTagOnDays));
        _cache.Set(longUrl, shortCode,
            TimeSpan.FromDays(_shortenerSetting.DefaultExpirationCachedTagOnDays));
    }

    private string UrlResponseCombination(string shortCode)
        => $"{_shortenerSetting.BaseServiceUrl}/{shortCode}";
}