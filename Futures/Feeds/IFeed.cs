// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public interface IFeed
{
    FeedKind FeedKind { get; }
    Tag Tag { get; }

    Task HandleTickAsync(Tick tick);
}