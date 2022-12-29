// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class IntervalFeed : IFeed
{
    private int candleId = 0;
    private OHLC ohlc = null!;
    private TickOn? lastCloseOn = null;

    public event EventHandler<CandleArgs>? OnCandle;

    public IntervalFeed(Asset asset, Session session, Interval interval)
    {
        Asset = asset.MayNot().BeNull();
        Session = session.MayNot().BeNull();
        Interval = interval.Must().BeEnumValue();
    }

    public Asset Asset { get; }
    public Session Session { get; }
    public Interval Interval { get; }

    public FeedKind FeedKind => FeedKind.Interval;

    public void HandleTick(Tick tick, bool isLastTick)
    {
        var intervalOn = tick.TickOn.ToIntervalOn(Interval);

        if (isLastTick)
        {
            var candle = new Candle(Asset, candleId++, tick.TickOn,
                ohlc.Open, ohlc.High, ohlc.Low, ohlc.Close, false);

            OnCandle?.Invoke(this, new CandleArgs(candle, tick));

            return;
        }

        if (!Session.InSession(intervalOn))
            return;

        var closeOn = TickOn.From(intervalOn);

        if (!lastCloseOn.HasValue)
        {
            ohlc = new OHLC(closeOn, tick.Price);
        }
        else if (closeOn != lastCloseOn)
        {
            var candle = new Candle(Asset, candleId++, ohlc.CloseOn,
                ohlc.Open, ohlc.High, ohlc.Low, ohlc.Close, true);

            OnCandle?.Invoke(this, new CandleArgs(candle, tick));

            ohlc = new OHLC(closeOn, tick.Price);
        }
        else
        {
            ohlc.Adjust(tick);
        }

        lastCloseOn = closeOn;
    }
}