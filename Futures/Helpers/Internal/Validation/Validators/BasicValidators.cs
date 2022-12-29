// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Numerics;

namespace SquidEyes.Futures;

internal static class BasicValidators
{
    public static T BeDefault<T>(this Must<T> m)
    {
        return m.ThrowErrorIfNotIsValid(
            v => v.IsDefault(),
            v => $"be a default({typeof(T).FullName})");
    }

    public static T BeDefault<T>(this MayNot<T> m)
    {
        return m.ThrowErrorIfNotIsValid(
            v => !v.IsDefault(),
            v => $"be a default({typeof(T).FullName})");
    }

    public static T BeZero<T>(this Must<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v == T.Zero,
            v => "be zero");
    }

    public static T BeZero<T>(this MayNot<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v == T.Zero,
            v => "be zero");
    }

    public static T BeGreaterThan<T>(this Must<T> m, T value)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v > value,
            v => $"be greater than {value}");
    }

    public static T BeGreaterThanOrEqualTo<T>(this Must<T> m, T value)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v >= value,
            v => $"be greater than or equal to{value}");
    }

    public static T BeLessThan<T>(this Must<T> m, T value)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v < value,
            v => $"be less than {value}");
    }

    public static T BeLessThanOrEqualTo<T>(this Must<T> m, T value)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v <= value,
            v => $"be less than or equal to{value}");
    }

    public static T BePositive<T>(this Must<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v > T.Zero,
            v => "be greater than zero");
    }

    public static T BePositiveOrZero<T>(this Must<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v >= T.Zero,
            v => "be greater than or equal to zero");
    }

    public static T BeNegative<T>(this Must<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v < T.Zero,
            v => "be less than zero");
    }

    public static T BeNegativeOrZero<T>(this Must<T> m)
        where T : INumber<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v <= T.Zero,
            v => "be less than or equal to zero");
    }

    public static T BeEnumValue<T>(this Must<T> m)
        where T : Enum
    {
        return m.ThrowErrorIfNotIsValid(
            v => v.IsEnumValue(),
            v => $"be a defined {typeof(T)} value");
    }

    public static T BeBetween<T>(this Must<T> m, T minValue, T maxValue)
        where T : IComparable<T>
    {
        if (maxValue.CompareTo(minValue) < 0)
            throw new InvalidOperationException($"{maxValue} < {minValue}");

        return m.ThrowErrorIfNotIsValid(
            v => v.CompareTo(minValue) >= 0 && v.CompareTo(maxValue) <= 0,
            v => $"be >= {minValue} and <= {maxValue}");
    }

    public static T BeNonDefaultAndValid<T>(this Must<T> m)
        where T : IValidatable
    {
        m.Value.MayNot().BeDefault();
        m.Value.Validate();

        return m.Value;
    }

    public static T BeNull<T>(this Must<T> m)
        where T : class
    {
        return m.ThrowErrorIfNotIsValid(
            v => v is null,
            v => "be null");
    }

    public static T BeNull<T>(this MayNot<T> m)
        where T : class
    {
        return m.ThrowErrorIfNotIsValid(
            v => v is not null,
            v => "be null");
    }

    public static T BeEmpty<T, V>(this MayNot<T> m)
        where T : IEnumerable<V>
    {
        return m.ThrowErrorIfNotIsValid(
            v => v.IsNotEmpty(),
            v => "be null, empty, or contain one or more default items.");
    }

    public static string BeNullOrWhitespace(this MayNot<string> m)
    {
        return m.ThrowErrorIfNotIsValid(
            v => !string.IsNullOrWhiteSpace(v),
            v => "be null, String.Empty or be comprised of whitespace characters");
    }

    public static T Be<T>(this Must<T> m, T value)
        where T : IEquatable<T>
    {
        return m.ThrowErrorIfNotIsValid(
            v => m.Value.Equals(value),
            v => "be equal to");
    }

    public static T Be<T>(this Must<T> m, Func<T, bool> isValid)
    {
        if (isValid is null)
        {
            throw new InvalidOperationException(
                "An \"isValid\" expression must be supplied.");
        }

        return m.ThrowErrorIfNotIsValid(
            v => isValid(v),
            "Value does not fall within the expected range.");
    }
}