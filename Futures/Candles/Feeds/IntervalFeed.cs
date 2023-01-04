// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class IntervalFeed : IFeed
{
    private readonly long intervalTicks;
    private readonly DateTime minTickOnValue;
    private readonly DateTime maxTickOnValue;

    private int candleId = 0;
    private OHLC ohlc = null!;
    private TickOn? lastCloseOn = null;

    public event EventHandler<CandleArgs>? OnCandle;

    public IntervalFeed(Asset asset, TradeDate tradeDate, int seconds)
    {
        Asset = asset.MayNot().BeNull();
        TradeDate = tradeDate.MayNot().BeDefault();
        Seconds = seconds.Must().Be(v => v.IsInterval());

        minTickOnValue = TradeDate.AsDateTime().Add(
            Known.TickOnOffsets.From);

        maxTickOnValue = TradeDate.AsDateTime().Add(
            Known.TickOnOffsets.Until);

        intervalTicks = TimeSpan.FromSeconds(seconds).Ticks;
    }

    public Asset Asset { get; }
    public TradeDate TradeDate { get; }
    public int Seconds { get; }

    public FeedKind FeedKind => FeedKind.Interval;

    public void HandleTick(Tick tick, bool isLastTick)
    {
        tick.TickOn.AsDateTime().Must()
            .BeBetween(minTickOnValue, maxTickOnValue);

        var dateTime = tick.TickOn.AsDateTime().Ticks.AsFunc(t => 
            new DateTime(t - (t % intervalTicks)).AddTicks(intervalTicks));

        if (dateTime > maxTickOnValue)
            return;

        var closeOn = TickOn.From(dateTime);

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

    public Candle GetOpenCandle(Tick tick)
    {
        tick.MayNot().BeDefault();

        if (ohlc == null)
        {
            throw new InvalidOperationException(
                "The \"HandleTick\" method has yet to be called once!");
        }

        return new Candle(Asset, candleId++, tick.TickOn,
            ohlc.Open, ohlc.High, ohlc.Low, ohlc.Close, false);
    }
}