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
            .Enrich.WithActivityDatadogFormat()
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        log.Information(@"Has a TraceId and SpanId property");

        Assert.NotNull(evt);

        var traceId = _testActivity.TraceId.ToString().TraceIdToDatadogFormat();
        var spanId = _testActivity.SpanId.ToString().SpanIdToDatadogFormat();

        Assert.AreEqual(traceId, (string)evt.Properties["dd.trace_id"].LiteralValue());
        Assert.AreEqual(spanId, (string)evt.Properties["dd.span_id"].LiteralValue());
    }
}