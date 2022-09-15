using System;
using NUnit.Framework;
using Serilog.Enrichers.Activity.Tests.Support;
using Serilog.Events;

namespace Serilog.Enrichers.Activity.Tests.Enrichers;

public class TraceEnricherTests
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
    public void TraceEnricherIsApplied()
    {
        LogEvent evt = null;
        var log = new LoggerConfiguration()
            .Enrich.WithTraceDatadogFormat()
            .WriteTo.Sink(new DelegatingSink(e => evt = e))
            .CreateLogger();

        log.Information(@"Has a TraceId property");

        Assert.NotNull(evt);

        var traceId = _testActivity.TraceId.ToString();
        var ddTraceId = Convert.ToUInt64(traceId?[16..], 16).ToString();

        Assert.AreEqual(ddTraceId, (string)evt.Properties["dd.trace_id"].LiteralValue());
    }
}