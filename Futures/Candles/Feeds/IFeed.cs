// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public interface IFeed
{
    FeedKind FeedKind { get; }

    event EventHandler<CandleArgs>? OnCandle;

    void HandleTick(Tick tick, bool isLastTick);
}