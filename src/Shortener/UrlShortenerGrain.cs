namespace Shortener;

public interface IUrlShortenerGrain : IGrainWithStringKey
{
    Task SetUrl(string longUrl);

    Task<string> GetUrl();
}


public sealed class UrlShortenerGrain(
    [PersistentState(stateName: "url", storageName: "urls")] IPersistentState<UrlDetails> state)
    : Grain, IUrlShortenerGrain
{

    public Task<string> GetUrl() =>
        Task.FromResult(state.State.LongUrl);

    public async Task SetUrl(string longUrl)
    {
        state.State = new()
        {
            ShortenedRouteSegment = this.GetPrimaryKeyString(),
            LongUrl = longUrl
        };

        await state.WriteStateAsync();
    }
}


[GenerateSerializer, Alias(nameof(UrlDetails))]
public sealed record class UrlDetails
{
    [Id(0)]
    public string LongUrl { get; set; } = "";

    [Id(1)]
    public string ShortenedRouteSegment { get; set; } = "";
}