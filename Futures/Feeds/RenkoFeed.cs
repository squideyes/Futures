//// Copyright (C) 2023 SquidEyes, LLC - All Rights Reserved
//// Proprietary and confidential
//// Unauthorized copying of this file, via any medium is strictly prohibited

//using SquidEyes.Fundamentals.Feeds;

//namespace SquidEyes.Squidvestor.Trading;

//public class WickoFeed : FeedBase
//{
//    private readonly float brickPoints;

//    private bool firstTick = true;
//    private Candle lastCandle = null!;
//    private int candleId = 0;

//    private DateTime closeOn;
//    private float open;
//    private float high;
//    private float low;
//    private float close;

//    private WickoFeed(WickoFeedArgs feedArgs, ICandleHandler candleHandler)
//        : base(FeedKind.Wicko, feedArgs, candleHandler)
//    {
//        brickPoints = Asset.Round(Asset.OneTick * feedArgs.BrickTicks.AsInt32());
//    }

//    private Candle GetNewCandle(float open,
//        float high, float low, float close, bool isClosed)
//    {
//        var candle = new Candle(Asset, candleId,
//            closeOn, open, high, low, close, isClosed);

//        if (isClosed)
//            candleId++;

//        return candle;
//    }

//    private async Task RisingAsync(Tick tick)
//    {
//        float limit;

//        while (close > (limit = Asset.Round(open + brickPoints)))
//        {
//            var candle = GetNewCandle(open, limit, low, limit, true);

//            lastCandle = candle;

//            await RaiseCandleAsync(
//                new CandleArgs(candle, tick, Name, Asset));

//            open = limit;
//            low = limit;
//        }
//    }

//    private async Task FallingAsync(Tick tick)
//    {
//        float limit;

//        while (close < (limit = Asset.Round(open - brickPoints)))
//        {
//            var candle = GetNewCandle(open, high, limit, limit, true);

//            lastCandle = candle;

//            await RaiseCandleAsync(
//                new CandleArgs(candle, tick, Name, Asset));

//            open = limit;
//            high = limit;
//        }
//    }

//    internal Candle GetOpenCandle() =>
//        GetNewCandle(open, high, low, close, false);

//    protected override async Task ProcessTickAsync(Tick tick, Asset asset)
//    {
//        if (firstTick)
//        {
//            firstTick = false;

//            closeOn = tick.DateTime;
//            open = tick.Price;
//            high = tick.Price;
//            low = tick.Price;
//            close = tick.Price;
//        }
//        else
//        {
//            closeOn = tick.DateTime;

//            if (tick.Price > high)
//                high = tick.Price;

//            if (tick.Price < low)
//                low = tick.Price;

//            close = tick.Price;

//            if (close > open)
//            {
//                if (lastCandle == null! || (lastCandle.Trend == Trend.Up))
//                {
//                    await RisingAsync(tick);

//                    return;
//                }

//                var limit = Asset.Round(lastCandle.Open + brickPoints);

//                if (close > limit)
//                {
//                    var candle = GetNewCandle(
//                        lastCandle.Open, limit, low, limit, true);

//                    lastCandle = candle;

//                    await RaiseCandleAsync(
//                        new CandleArgs(candle, tick, Name, Asset));

//                    open = limit;
//                    low = limit;

//                    await RisingAsync(tick);
//                }
//            }
//            else if (close < open)
//            {
//                if (lastCandle == null! || (lastCandle.Trend == Trend.Down))
//                {
//                    await FallingAsync(tick);

//                    return;
//                }

//                var limit = Asset.Round(lastCandle.Open - brickPoints);

//                if (close < limit)
//                {
//                    var candle = GetNewCandle(
//                        lastCandle.Open, high, limit, limit, true);

//                    lastCandle = candle;

//                    await RaiseCandleAsync(
//                        new CandleArgs(candle, tick, Name, Asset));

//                    open = limit;
//                    high = limit;

//                    await FallingAsync(tick);
//                }
//            }
//        }
//    }

//    public static WickoFeed Create(WickoFeedArgs feedArgs, ICandleHandler candleHandler)
//    {
//        feedArgs.Must().BeNonDefaultAndValid();
//        candleHandler.MayNot().BeNull();

//        return new WickoFeed(feedArgs, candleHandler);
//    }
//}