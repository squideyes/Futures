// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using static System.TimeSpan;

namespace SquidEyes.Futures;

internal static class FeedExtenders
{
    private static readonly Dictionary<Interval, long> intervals = new();

    static FeedExtenders()
    {
        foreach (var interval in Enum.GetValues<Interval>())
            intervals.Add(interval, (int)interval * TicksPerSecond);
    }

    public static DateTime ToIntervalOn(
        this TickOn value, Interval interval)
    {
        value.MayNot().BeDefault();
        interval.Must().BeEnumValue();

        var ticks = intervals[interval];

        return value.AsDateTime().Ticks.AsFunc(
            t => new DateTime(t - (t % ticks)).AddTicks(ticks));
    }
}