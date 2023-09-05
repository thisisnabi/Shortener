namespace Devblogs.Shortener.Interfaces;

public interface IShortCodeHandler
{
    Task<string> GenerateShortCodeAsync(string longUrl, int length);
}
