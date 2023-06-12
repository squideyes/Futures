using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public interface IFeed
{
    FeedKind FeedKind { get; }
    Tag Tag { get; }

    Task HandleTickAsync(Tick tick);
}