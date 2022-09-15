using System;

namespace Serilog.Enrichers.Activity;

public static class FormatExtensions
{
    public static string TraceIdToDatadogFormat(this string value)
    {
        var ddTraceId = Convert.ToUInt64(value[16..], 16).ToString();
        return ddTraceId;
    }

    public static string SpanIdToDatadogFormat(this string value)
    {
        var ddSpanId = Convert.ToUInt64(value, 16).ToString();
        return ddSpanId;
    }
}