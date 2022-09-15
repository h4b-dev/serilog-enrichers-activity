using System;
using System.Diagnostics;
using Serilog.Configuration;
using Serilog.Enrichers.Activity.Enrichers;

namespace Serilog.Enrichers.Activity;

/// <summary>
/// Extends <see cref="LoggerConfiguration"/> to add enrichers related to activity.
/// capabilities.
/// </summary>
public static class ActivityLoggerConfigurationExtensions
{
    /// <summary>
    /// Enrich log events with a trace and span property containing the current <see cref="Activity"/>.
    /// </summary>
    /// <param name="enrichmentConfiguration">Logger enrichment configuration.</param>
    /// <returns>Configuration object allowing method chaining.</returns>
    public static LoggerConfiguration WithActivityDatadogFormat(
        this LoggerEnrichmentConfiguration enrichmentConfiguration)
    {
        if (enrichmentConfiguration == null) throw new ArgumentNullException(nameof(enrichmentConfiguration));
        return enrichmentConfiguration.With<ActivityDatadogFormatEnricher>();
    }
}