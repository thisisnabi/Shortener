using Shortener.Filters;

namespace Shortener.UnitTests;
public class ShortenEndpointFilterTests
{
    [Theory]
    [InlineData("https://thisisnabi.dev")]
    [InlineData("https://meet.google.com/thisisnabi")]
    [InlineData("https://github.com/thisisnabi/Blogger/tree/main/src/Blogger.APIs")]
    public void IsValidUrl_ShouldReturnTrue_WhenUrlIsValid(string url)
    {
        // act 
        var result = ShortenEndpointFilter.IsValidUrl(url);

        // assert
        Assert.True(result);
    }


    [Theory]
    [InlineData("htdev")]
    [InlineData("http.google.com/thisisnabi")]
    [InlineData("httger/tree/main/src/Blogger.APIs")]
    public void IsValidUrl_ShouldReturnFalse_WhenUrlIsNotValid(string url)
    {
        // act 
        var result = ShortenEndpointFilter.IsValidUrl(url);

        // assert
        Assert.False(result);
    }
}
