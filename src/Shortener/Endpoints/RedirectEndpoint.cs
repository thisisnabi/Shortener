namespace Shortener.Endpoints;

public static class RedirectEndpoint
{
    public static void MapRedirectEndpoint(this IEndpointRouteBuilder endpoint)
    {
        endpoint.MapGet("/{short_code}", async (
        [FromRoute(Name = "short_code")] string ShortCode,
        ShortenUrlService shortenService,
        CancellationToken cancellationToken
        ) =>
        {
            var destinationUrl = await shortenService.GetDestinationUrlAsync(ShortCode, cancellationToken);

            return Results.Redirect(destinationUrl);
        });

    }
}
