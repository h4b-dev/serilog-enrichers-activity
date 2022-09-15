using Serilog.Events;

namespace Serilog.Enrichers.Activity.Tests.Support;

public static class Extensions
{
    public static object LiteralValue(this LogEventPropertyValue @this)
    {
        return ((ScalarValue)@this).Value;
    }
}