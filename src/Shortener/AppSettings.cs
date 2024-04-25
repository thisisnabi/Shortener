namespace Shortener;

public class AppSettings
{
    public MongoDbSetting MongoDbSetting { get; set; } = null!;

    public string BaseUrl { get; set; } = null!;

    public int ShortCodeLength { get; set; }
}


public class MongoDbSetting
{
    public string Host { get; set; } = null!;

    public string DatabaseName { get; set; } = null!;
}
