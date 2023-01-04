// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class TdRenkoFeed : IFeed
{
    private readonly Asset asset;
    private readonly float brickPoints;

    private Candle lastCandle = null!;
    private int candleId = 0;

    private float open;
    private float maxPrice;
    private float minPrice;

    public event EventHandler<CandleArgs>? OnCandle;

    private TdRenkoFeed(Asset asset, float brickPoints)
    {
        this.asset = asset;
        this.brickPoints = brickPoints;
    }

    public FeedKind FeedKind => FeedKind.TdRenko;

    public void HandleTick(Tick tick, bool isLastTick)
    {
        tick.MayNot().BeDefault();
        
        if (lastCandle == null!)
        {
            open = tick.Price;
            maxPrice = asset.Round(tick.Price + brickPoints);
            minPrice = asset.Round(tick.Price - brickPoints);

            var candle = new Candle(asset, candleId++, tick.TickOn,
                tick.Price, tick.Price, tick.Price, tick.Price, false);

            lastCandle = candle;
        }
        else
        {
            var last = lastCandle;

            var maxExceeded = tick.Price > maxPrice;
            var minExceeded = tick.Price < minPrice;

            if (maxExceeded || minExceeded)
            {
                var high = maxExceeded ? maxPrice : last.High;
                var low = minExceeded ? minPrice : last.Low;
                var close = maxExceeded ? maxPrice : minPrice;

                var candle = new Candle(asset, last.CandleId,
                    tick.TickOn, last.Open, high, low, close, true);

                lastCandle = candle;

                OnCandle?.Invoke(this, new CandleArgs(candle, tick));

                float bodyHigh = MathF.Max(last.Open, close);
                float bodyLow = MathF.Min(last.Open, close);

                open = asset.Round(Adjust(bodyHigh + bodyLow) / 2.0f);
                maxPrice = asset.Round(open + brickPoints);
                minPrice = asset.Round(open - brickPoints);

                candle = new Candle(asset, candleId++, tick.TickOn, open,
                    MathF.Max(open, tick.Price), MathF.Min(open, tick.Price), close, false);

                lastCandle = candle;
            }
            else
            {
                var candle = new Candle(asset, last.CandleId, tick.TickOn, last.Open,
                    tick.Price > last.High ? tick.Price : last.High,
                    tick.Price < last.Low ? tick.Price : last.Low, tick.Price, false);

                lastCandle = candle;
            }
        }
    }

    private float Adjust(float price)
    {
        return price % asset.OneTick != 0.0f
            ? MathF.Ceiling(price / asset.OneTick) * asset.OneTick
            : MathF.Round(price / asset.OneTick) * asset.OneTick;
    }

    public static TdRenkoFeed Create(Asset asset, BrickTicks brickTicks)
    {
        asset.MayNot().BeNull();
        brickTicks.MayNot().BeDefault();

        return new TdRenkoFeed(asset,
            asset.Round(asset.OneTick * brickTicks.AsInt32()));
    }
}