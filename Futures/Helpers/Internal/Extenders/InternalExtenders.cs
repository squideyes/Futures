// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text;

namespace SquidEyes.Futures;

internal static class InternalExtenders
{
    public static R AsFunc<V, R>(this V value, Func<V, R> func) => func(value);

    public static void AsAction<T>(this T value, Action<T> act) => act(value);

    public static bool IsBetween<T>(this T value, T minValue, T maxValue)
        where T : IComparable<T>
    {
        if (maxValue.CompareTo(minValue) < 0)
            throw new InvalidOperationException($"{maxValue} < {minValue}");

        return value.CompareTo(minValue) >= 0 
            && value.CompareTo(maxValue) <= 0;
    }

    public static void EnsurePathExists(this string fileName)
    {
        var path = Path.GetDirectoryName(fileName);

        if (!Directory.Exists(path))
            Directory.CreateDirectory(path!);
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