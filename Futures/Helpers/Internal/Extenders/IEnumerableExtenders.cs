namespace SquidEyes.Futures;

public static class IEnumerableExtenders
{
    public static bool IsNotEmpty<T>(this IEnumerable<T> value) =>
        value is not null && value.Any() && !value.Any(v => v.IsDefault());
}