//// Copyright (C) 2023 SquidEyes, LLC - All Rights Reserved
//// Proprietary and confidential
//// Unauthorized copying of this file, via any medium is strictly prohibited

//using SquidEyes.Fundamentals.Feeds;

//namespace SquidEyes.Squidvestor.Trading;

//public class TdRenkoFeed : FeedBase
//{
//    private readonly float brickPoints;

//    private Candle lastCandle = null!;
//    private int candleId = 0;

//    private float open;
//    private float maxPrice;
//    private float minPrice;

//    private TdRenkoFeed(TdRenkoFeedArgs feedArgs, ICandleHandler candleHandler)
//        : base(FeedKind.TdRenko, feedArgs, candleHandler)
//    {
//        brickPoints = Asset.Round(Asset.OneTick * feedArgs.BrickTicks.AsInt32());
//    }

//    protected override async Task ProcessTickAsync(Tick tick, Asset asset)
//    {
//        tick.MayNot().BeDefault();

//        if (lastCandle == null!)
//        {
//            open = tick.Price;
//            maxPrice = Asset.Round(tick.Price + brickPoints);
//            minPrice = Asset.Round(tick.Price - brickPoints);

//            var candle = new Candle(Asset, candleId++, tick.DateTime,
//                tick.Price, tick.Price, tick.Price, tick.Price, false);

//            lastCandle = candle;
//        }
//        else
//        {
//            var last = lastCandle;

//            var maxExceeded = tick.Price > maxPrice;
//            var minExceeded = tick.Price < minPrice;

//            if (maxExceeded || minExceeded)
//            {
//                var high = maxExceeded ? maxPrice : last.High;
//                var low = minExceeded ? minPrice : last.Low;
//                var close = maxExceeded ? maxPrice : minPrice;

//                var candle = new Candle(Asset, last.CandleId,
//                    tick.DateTime, last.Open, high, low, close, true);

//                lastCandle = candle;

//                await RaiseCandleAsync(
//                    new CandleArgs(candle, tick, Name, Asset));

//                var bodyHigh = MathF.Max(last.Open, close);
//                var bodyLow = MathF.Min(last.Open, close);

//                open = Asset.Round(Adjust(bodyHigh + bodyLow) / 2.0f);
//                maxPrice = Asset.Round(open + brickPoints);
//                minPrice = Asset.Round(open - brickPoints);

//                high = MathF.Max(open, tick.Price);
//                low = MathF.Min(open, tick.Price);

//                candle = new Candle(Asset, candleId++,
//                    tick.DateTime, open, high, low, close, false);

//                lastCandle = candle;
//            }
//            else
//            {
//                var high = tick.Price > last.High ? tick.Price : last.High;
//                var low = tick.Price < last.Low ? tick.Price : last.Low;

//                var candle = new Candle(Asset, last.CandleId, tick.DateTime,
//                    last.Open, high, low, tick.Price, false);

//                lastCandle = candle;
//            }
//        }
//    }

//    private float Adjust(float price)
//    {
//        return price % Asset.OneTick != 0.0f
//            ? MathF.Ceiling(price / Asset.OneTick) * Asset.OneTick
//            : MathF.Round(price / Asset.OneTick) * Asset.OneTick;
//    }

//    public static TdRenkoFeed Create(TdRenkoFeedArgs feedArgs, ICandleHandler candleHandler)
//    {
//        feedArgs.Must().BeNonDefaultAndValid();
//        candleHandler.MayNot().BeNull();

//        return new TdRenkoFeed(feedArgs, candleHandler);
//    }
//}