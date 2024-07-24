using System.Diagnostics.Metrics;

namespace Shortener.Diagnostics;

public sealed class ShortenDiagnostic
{
    public const string MeterName = "ThisIsNabi.Shorten";

    public const string RedirectionMetricName = "ThisIsNabi.Shorten.Redirection";
    public const string FailedRedirectionMetricName = "ThisIsNabi.Shorten.Redirection.Failed";

    private readonly Counter<long> _redirectionCounter;
    private readonly Counter<long> _failedRedirectionCounter;

    public ShortenDiagnostic(IMeterFactory meterFactory)
    {
        var meter = meterFactory.Create(MeterName);
        _redirectionCounter = meter.CreateCounter<long>(RedirectionMetricName);
        _failedRedirectionCounter = meter.CreateCounter<long>(FailedRedirectionMetricName);
    }

    private const string RedirectionTagName = "Label";
    public void AddRedirection(string title)
        => _redirectionCounter.Add(1, new KeyValuePair<string, object?>(RedirectionTagName, title));

    public void AddFailedRedirection()
        => _failedRedirectionCounter.Add(1);
}
