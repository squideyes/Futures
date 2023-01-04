// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

internal static class FeedExtenders
{
    public static TickOn ToIntervalOn(this TickOn value, int seconds)
    {
        var ticks = TimeSpan.FromSeconds(seconds).Ticks;

        return TickOn.From(value.AsDateTime().Ticks.AsFunc(
            t => new DateTime(t - (t % ticks)).AddTicks(ticks)));
    }

    public static bool IsIntervalOn(this TickOn value, int seconds)
    {
        var ticks = TimeSpan.FromSeconds(seconds).Ticks;

        return value.AsDateTime().Ticks % ticks == 0;
    }
}