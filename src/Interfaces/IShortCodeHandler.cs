namespace Devblogs.Shortener.Interfaces;

public interface IShortCodeHandler
{
    Task<string> GenerateAsync(string longUrl, int length);
}
