// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public class Feed : FeedBase
{
    private int barId = 0;
    private Bar bar = null!;
    private DateTime? lastCloseOn = null;

    private Feed(FeedArgs args, IBarHandler barHandler)
        : base(FeedKind.Interval, args, barHandler)
    {
    }

    protected override async Task ProcessTickAsync(Tick tick)
    {
        if (!Session!.IsInSession(tick.TickOn))
            return;

        var closeOn = BarSpec.GetIntervalCloseOn(tick.TickOn);

        if (!lastCloseOn.HasValue)
        {
            bar = new Bar(tick, barId++, BarSpec, Asset!);
        }
        else if (closeOn != lastCloseOn)
        {
            await BarClosedAsync(
                new BarArgs(bar, tick, Tag, Asset!));

            bar = new Bar(tick, barId++, BarSpec, Asset!);
        }
        else
        {
            bar.Adjust(tick);
        }

        lastCloseOn = closeOn;
    }

    public static Feed Create(FeedArgs args, IBarHandler barHandler)
    {
        args.MayNot().BeNull();
        barHandler.MayNot().BeNull();

        return new Feed(args, barHandler);
    }
}