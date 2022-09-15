using System;
using NUnit.Framework;
using Serilog.Enrichers.Activity.Tests.Support;
using Serilog.Events;

namespace Serilog.Enrichers.Activity.Tests.Enrichers;

public class SpanEnricherTests
{
    private System.Diagnostics.Activity _testActivity;

    [OneTimeSetUp]
    public void Init()
    {
        _testActivity = new System.Diagnostics.Activity("UnitTest").Start();
    }

    [OneTimeTearDown]
    public void Dispose()
    {
        _testActivity.Stop();
    }

    [Test]
    public void SpanEnricherIsApplied()
    {
        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.WithSpanDatadogFormat()
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        log.Information(@"Has a SpanId property");

        Assert.NotNull(evt);

        var spanId = _testActivity.SpanId.ToString();
        var ddSpanId = Convert.ToUInt64(spanId, 16).ToString();

        Assert.AreEqual(ddSpanId, (string)evt.Properties["dd.span_id"].LiteralValue());
    }
}