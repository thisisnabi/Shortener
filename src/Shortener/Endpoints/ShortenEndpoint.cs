namespace Shortener.Endpoints;

public static class ShortenEndpoint
{
    public static void MapShortenEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapPost("/shorten", async (
         [FromBody] ShortenRequest request,
         ShortenUrlService shortenService,
         CancellationToken cancellationToken
         ) =>
        {

            return await shortenService.GenerateShortenUrlAsync(request.Url, cancellationToken);
        }).AddEndpointFilter<ShortenEndpointFilter>();


    }
}
