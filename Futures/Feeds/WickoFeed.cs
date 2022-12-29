// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class WickoFeed : IFeed
{
    private readonly Asset asset;
    private readonly float brickPoints;

    private bool firstTick = true;
    private Candle lastCandle = null!;
    private int candleId = 0;

    private TickOn closeOn;
    private float open;
    private float high;
    private float low;
    private float close;

    public event EventHandler<CandleArgs>? OnCandle;

    private WickoFeed(Asset asset, float brickPoints)
    {
        this.asset = asset;
        this.brickPoints = brickPoints;
    }

    public FeedKind FeedKind => FeedKind.Wicko;

    private Candle GetNewCandle(float open,
        float high, float low, float close, bool isClosed)
    {
        var candle = new Candle(asset, candleId,
            closeOn, open, high, low, close, isClosed);

        if (isClosed)
            candleId++;

        return candle;
    }

    private void Rising(Tick tick)
    {
        float limit;

        while (close > (limit = asset.Round(open + brickPoints)))
        {
            var candle = GetNewCandle(open, limit, low, limit, true);

            lastCandle = candle;

            OnCandle?.Invoke(this, new CandleArgs(candle, tick));

            open = limit;
            low = limit;
        }
    }

    private void Falling(Tick tick)
    {
        float limit;

        while (close < (limit = asset.Round(open - brickPoints)))
        {
            var candle = GetNewCandle(open, high, limit, limit, true);

            lastCandle = candle;

            OnCandle?.Invoke(this, new CandleArgs(candle, tick));

            open = limit;
            high = limit;
        }
    }

    internal Candle GetOpenCandle() =>
        GetNewCandle(open, high, low, close, false);

    public void HandleTick(Tick tick, bool isLastTick)
    {
        if (firstTick)
        {
            firstTick = false;

            closeOn = tick.TickOn;
            open = tick.Price;
            high = tick.Price;
            low = tick.Price;
            close = tick.Price;
        }
        else
        {
            closeOn = tick.TickOn;

            if (tick.Price > high)
                high = tick.Price;

            if (tick.Price < low)
                low = tick.Price;

            close = tick.Price;

            if (close > open)
            {
                if (lastCandle == null! || (lastCandle.Trend == Trend.Up))
                {
                    Rising(tick);

                    return;
                }

                var limit = asset.Round(lastCandle.Open + brickPoints);

                if (close > limit)
                {
                    var candle = GetNewCandle(
                        lastCandle.Open, limit, low, limit, true);

                    lastCandle = candle;

                    OnCandle?.Invoke(this, new CandleArgs(candle, tick));

                    open = limit;
                    low = limit;

                    Rising(tick);
                }
            }
            else if (close < open)
            {
                if (lastCandle == null! || (lastCandle.Trend == Trend.Down))
                {
                    Falling(tick);

                    return;
                }

                var limit = asset.Round(lastCandle.Open - brickPoints);

                if (close < limit)
                {
                    var candle = GetNewCandle(
                        lastCandle.Open, high, limit, limit, true);

                    lastCandle = candle;

                    OnCandle?.Invoke(this, new CandleArgs(candle, tick));

                    open = limit;
                    high = limit;

                    Falling(tick);
                }
            }
        }
    }

    public static WickoFeed Create(Asset asset, BrickTicks brickTicks)
    {
        asset.MayNot().BeNull();
        brickTicks.MayNot().BeDefault();

        return new WickoFeed(asset,
            asset.Round(asset.OneTick * brickTicks.AsInt32()));
    }
}