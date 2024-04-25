using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using Shortener.Contracts;
using System.Text.RegularExpressions;

namespace Shortener.Filters;

public class ShortenEndpointFilter : IEndpointFilter
{
    private const string pattern = @"^(https?|ftp)://[^\s/$.?#].[^\s]*$";

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    { 
        var request = context.GetArgument<ShortenRequest>(0);

        if (IsValidUrl(request.Url))
        {
            return await next(context);
        }
  
        return Results.BadRequest(Constants.Validation.InvalidShortenRequestUrl);
    }

    private static bool IsValidUrl(string url)
       => Regex.IsMatch(url, pattern, RegexOptions.IgnoreCase);
}
