using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.Activity.Enrichers;

/// <summary>
/// Enriches log events with a Trace property containing the current <see cref="Activity.TraceId"/>.
/// </summary>
public class ActivityDatadogFormatEnricher : ILogEventEnricher
{
    /// <summary>
    /// The property name added to enriched log events.
    /// </summary>
    private const string TraceIdPropertyName = "dd.trace_id";

    private const string SpanIdPropertyName = "dd.span_id";

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var activity = System.Diagnostics.Activity.Current;
        if (activity is null) return;

        var traceProperty = propertyFactory.CreateProperty(TraceIdPropertyName, GetTraceId(activity));
        var spanProperty = propertyFactory.CreateProperty(SpanIdPropertyName, GetSpanId(activity));
        logEvent.AddPropertyIfAbsent(traceProperty);
        logEvent.AddPropertyIfAbsent(spanProperty);
    }

    private static string GetTraceId(System.Diagnostics.Activity activity)
    {
        var traceId = activity.TraceId.ToString().TraceIdToDatadogFormat();
        return traceId;
    }

    private static string GetSpanId(System.Diagnostics.Activity activity)
    {
        var spanId = activity.SpanId.ToString().SpanIdToDatadogFormat();
        return spanId;
    }
}