// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

internal static class FeedExtenders
{
    public static TickOn ToCloseOn(this TickOn value, int seconds)
    {
        value.MayNot().BeDefault();
        seconds.Must().Be(v => v.IsInterval());

        var interval = TimeSpan.FromSeconds(seconds).Ticks;

        return TickOn.From(value.AsDateTime().Ticks.AsFunc(
            t => new DateTime(t - (t % interval)).AddTicks(interval)));
    }
}