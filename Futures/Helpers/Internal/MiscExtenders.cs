using System.Text;

namespace SquidEyes.Futures;

internal static class MiscExtenders
{
    public static bool IsPeriodType<T>(this T value)
    {
        return value switch
        {
            TimeSpan _ => true,
            TimeOnly _ => true,
            _ => false
        };
    }

    public static void AppendDelimited(
        this StringBuilder sb, object value, char delimiter = ',')
    {
        if (sb.Length > 0)
            sb.Append(delimiter);

        sb.Append(value);
    }
}
