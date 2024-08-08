using Grpc.Core;
using Shortener.GrpcServices; 

namespace Shortener.Services;

public class ShortenService(ShortenUrlService shortenService)  :  ShortenUrl.ShortenUrlBase
{
    private readonly ShortenUrlService _shortenUrlService = shortenService;


    public override async Task<ShortenUrlResponse> GetShortenUrl(ShortenUrlRequest request, ServerCallContext context)
    {
        var ShortenUrl = await  _shortenUrlService.GenerateShortenUrlAsync(request.LongUrl, context.CancellationToken);
         
        return new ShortenUrlResponse { ShortenUrl = ShortenUrl };
    }
}
