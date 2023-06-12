using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public abstract class FeedBase : IFeed
{
    private readonly IBarHandler barHandler;

    private readonly int tickSkip;

    private int tickCount = 0;

    internal FeedBase(FeedKind feedKind, 
        FeedArgs feedArgs, IBarHandler barHandler)
    {
        FeedKind = feedKind;
        Tag = feedArgs.Tag;
        BarSpec = feedArgs.BarSpec;
        Asset = feedArgs.Asset!;
        Session = feedArgs.Session!;
        tickSkip = feedArgs.TickSkip;
        this.barHandler = barHandler;
    }

    public FeedKind FeedKind { get; }
    public Tag Tag { get; }

    protected BarSpec BarSpec { get; }
    protected Asset Asset { get; }
    protected Session Session { get; }

    public async Task HandleTickAsync(Tick tick)
    {
        if (++tickCount > tickSkip)
            return;

        await ProcessTickAsync(tick);
    }

    protected async Task BarClosedAsync(BarArgs args) =>
        await barHandler.HandleBarAsync(args);

    protected abstract Task ProcessTickAsync(Tick tick);
}