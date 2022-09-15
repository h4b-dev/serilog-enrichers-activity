using System;
using System.Diagnostics;
using Serilog.Core;
using Serilog.Events;

namespace Serilog.Enrichers.Activity.Enrichers;

/// <summary>
/// Enriches log events with a Span property containing the current <see cref="Activity.SpanId"/>.
/// </summary>
public class SpanDatadogFormatEnricher : ILogEventEnricher
{
    /// <summary>
    /// The property name added to enriched log events.
    /// </summary>
    private const string SpanIdPropertyName = "dd.span_id";

    /// <summary>
    /// Enrich the log event.
    /// </summary>
    /// <param name="logEvent">The log event to enrich.</param>
    /// <param name="propertyFactory">Factory for creating new properties to add to the event.</param>
    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        var property = propertyFactory.CreateProperty(SpanIdPropertyName, GetSpanId());
        logEvent.AddPropertyIfAbsent(property);
    }

    private static string GetSpanId()
    {
        using var activity = System.Diagnostics.Activity.Current;
        var spanId = activity?.SpanId.ToString();
        var ddSpanId = Convert.ToUInt64(spanId, 16).ToString();
        return ddSpanId;
    }
}