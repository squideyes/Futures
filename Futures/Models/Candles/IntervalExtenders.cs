// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public static class IntervalExtenders
{
    public static string ToCode(this Interval value)
    {
        return value switch
        {
            Interval.FifteenSecond => "15S",
            Interval.ThirtySecond => "30S",
            Interval.OneMinute => "01M",
            Interval.TwoMinute => "02M",
            Interval.ThreeMinute => "03M",
            Interval.FiveMinute => "05M",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static Interval ToInterval(this string value)
    {
        if (!value.TryGetInterval(out Interval interval))
            throw new ArgumentOutOfRangeException(value);

        return interval;
    }

    public static bool TryGetInterval(
        this string value, out Interval size)
    {
        size = value switch
        {
            "15S" => Interval.FifteenSecond,
            "30S" => Interval.ThirtySecond,
            "01M" => Interval.OneMinute,
            "02M" => Interval.TwoMinute,
            "03M" => Interval.ThreeMinute,
            "05M" => Interval.FiveMinute,
            _ => default
        };

        return size != default;
    }
}