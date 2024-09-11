namespace Shortener;

public interface IUrlShortenerGrain : IGrainWithStringKey
{
    Task SetLongUrl(string longUrl);

    Task<string> GetLongUrl();
}

[GenerateSerializer, Alias(nameof(UrlDetails))]
public sealed record class UrlDetails
{
    [Id(0)]
    public string LongUrl { get; set; } = "";

    [Id(1)]
    public string ShortCode { get; set; } = "";
}

public sealed class UrlShortenerGrain([PersistentState(stateName: "url", storageName: "urls")] IPersistentState<UrlDetails> state) : Grain, IUrlShortenerGrain
{

    public Task<string> GetLongUrl() => 
        Task.FromResult(state.State.LongUrl);

    public async Task SetLongUrl(string longUrl)
    {
        state.State = new()
        {
            ShortCode = this.GetPrimaryKeyString(),
            LongUrl = longUrl
        };

        await state.WriteStateAsync();
    }
}
