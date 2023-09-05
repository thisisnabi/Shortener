namespace Devblogs.Shortener.Handlers;

public class ShortCodeHandler : IShortCodeHandler
{
    private readonly ITagRepository _tagRepository;
    public ShortCodeHandler(ITagRepository tagRepository)
    {
        _tagRepository = tagRepository;
    }

    public async Task<string> GenerateAsync(string longUrl, int length)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] hashBytes = md5.ComputeHash(Encoding.UTF8.GetBytes(longUrl));
            string hashCode = BitConverter.ToString(hashBytes)
                                            .Replace(oldValue: "-", newValue: "")
                                                .ToLower();

            for (int i = 0; i <= hashCode.Length - length; i++)
            {
                string candidateCode = hashCode.Substring(i, length);
                var hasCode = await _tagRepository.HasCodeAsync(candidateCode);

                if (!hasCode)
                {
                    return candidateCode;
                }
            }

            throw new Exception(Constants.Data.ExceptionMessage.FailedGenerateUniqCode);
        }
    }
}
