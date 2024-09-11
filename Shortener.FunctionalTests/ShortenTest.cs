using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Shortener.FunctionalTests;

public class ShortenTest : IClassFixture<WebApplicationFactory<Program>>
{
    private WebApplicationFactory<Program> _factory;
    public ShortenTest(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }


    [Fact]
    public async Task GivenAValidUrl_WhenShortenApiIsCalled_ThenReturnOkStatus()
    {
        //Arrange
        string url = "http://google.com";

        var client = _factory.CreateClient();

        // Act
        var request = await client.GetAsync($"/shorten?url={url}");

        // Assert
        var result = await request.Content.ReadAsStringAsync();

        Assert.Equal(System.Net.HttpStatusCode.OK, request.StatusCode);
    }

    [Fact]
    public async Task GivenAShortCode_WhenShortenApiIsCalled_ThenReturnBadRequest()
    {
        // Arrange
        string url = "http://google.com/";

        var client = _factory.CreateClient();

        // Act 1
        var shortenResponse = await client.GetAsync($"/shorten?url={url}");

        // Assert 1
        Assert.Equal(System.Net.HttpStatusCode.OK, shortenResponse.StatusCode);

        var shortenResult = await shortenResponse.Content.ReadAsStringAsync();
        string lastSegment = shortenResult.Split('/').Last();
        var shortCode = lastSegment.Trim('"');

        // Act 2
        var shortCodeRequest = await client.GetAsync($"short_code={shortCode}");

        // Assert 2
        string response = await shortCodeRequest.Content.ReadAsStringAsync();
        Assert.Equal(System.Net.HttpStatusCode.BadRequest, shortCodeRequest.StatusCode);
    }

    [Fact]
    public async Task GivenAAShortCode_WhenShortenApiIsCalled_ThenReturnSameUrl()
    {
        // Arrange
        string url = "http://google.com/";

        var client = _factory.CreateClient();

        // Act 1
        var shortenResponse = await client.GetAsync($"/shorten?url={url}");

        // Assert 1
        Assert.Equal(System.Net.HttpStatusCode.OK, shortenResponse.StatusCode);

        var shortenResult = await shortenResponse.Content.ReadAsStringAsync();
        string lastSegment = shortenResult.Split('/').Last();
        var shortCode = lastSegment.Trim('"');

        // Act 2
        var shortCodeRequest = await client.GetAsync($"{shortCode}");

        var redirectUrl = shortCodeRequest!.RequestMessage!.RequestUri!.ToString();

        Assert.Equal(redirectUrl, url);
    }

}
