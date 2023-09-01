namespace Devblogs.Shortener;

public static class Constants
{
    public static class Data
    {
        public static class ExceptionMessage
        {
            public const string SearchLongUrl = "Long url";
            public const string SearchShortUrl = "Short url";
            public const string FailedGenerateUniqCode = "Failed to generate a unique short code.";
        }
        
        public static class EndPointFilterMessages
        {
            public const string InvalidUrl = "Entred Url is not valid.";
        }
    }
}
