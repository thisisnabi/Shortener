using System.Security.Cryptography;
using System.Text;
using FluentAssertions;
using MongoDB.Driver;

namespace Shortener.UnitTests;

public class CodeGeneratorTests
{
    [Fact]
    public void GenerateCode_ShouldReturnCorrectHash_WhenGivenValidUrl()
    {
        var longUrl = "https://thisisnabi.dev/system-design";

        var result = Program.GenerateCode(longUrl);

        result.Should().NotBeNullOrEmpty();
        result.Should().BeLowerCased();
    }

    [Fact]
    public void GenerateCode_ShouldReturnDiffrentHash_ForDiffrentUrls()
    {
        var longUrl1 = "https://thisisnabi.dev/system-design";
        var longUrl2 = "https://thisisnabi.dev/affiliant";

        var result1 = Program.GenerateCode(longUrl1);
        var result2 = Program.GenerateCode(longUrl2);

        result1.Should().NotBe(result2);
    }
}
