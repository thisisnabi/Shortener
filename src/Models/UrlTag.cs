namespace Devblogs.Shortener.Models;

public sealed class Tag
{
    public const string TableName = "Tags";

    public long Id { get; set; }
    public string ShortCode { get; set; }
    public string LongUrl { get; set; }

    public Tag(string shortCode, string longUrl)
    {
        ShortCode = shortCode;
        LongUrl = longUrl;
    }

    public static Tag Create(string shortCode, string longUrl)
        => new(shortCode, longUrl);
}
