using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.Activity.Enrichers;

/// <summary>
/// Enriches log events with a Trace property containing the current <see cref="Activity.TraceId"/>.
/// </summary>
public class TraceDatadogFormatEnricher : ILogEventEnricher
{
    /// <summary>
    /// The property name added to enriched log events.
    /// </summary>
    private const string TraceIdPropertyName = "dd.trace_id";

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var property = propertyFactory.CreateProperty(TraceIdPropertyName, GetTraceId());
        logEvent.AddPropertyIfAbsent(property);
    }

    private static string GetTraceId()
    {
        using var activity = System.Diagnostics.Activity.Current;
        var traceId = activity?.TraceId.ToString();
        var ddTraceId = Convert.ToUInt64(traceId?[16..], 16).ToString();
        return ddTraceId;
    }
}