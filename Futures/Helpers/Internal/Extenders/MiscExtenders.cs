using System.Text;

namespace SquidEyes.Futures;

internal static class MiscExtenders
{
    public static void EnsurePathExists(this string fileName)
    {
        var path = Path.GetDirectoryName(fileName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path!);
    }

    public static string ToCode(this Source value)
    {
        return value switch
        {
            Source.Kibot => "KB",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static Source ToSource(this string value)
    {
        return value switch
        {
            "KB" => Source.Kibot,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static bool IsEnumValue<T>(this T value)
        where T : Enum
    {
        return Enum.IsDefined(typeof(T), value);
    }

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

    public static bool IsDefault<T>(this T value) =>
        Equals(value, default);
}
