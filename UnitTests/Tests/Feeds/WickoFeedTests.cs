// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

//using SquidEyes.Futures;

//namespace SquidEyes.UnitTests;

//public class WickoFeedTests
//{
//    private static (WickoFeed, List<Candle>) GetFeedAndCandles()
//    {
//        var feed = WickoFeed.Create(
//            Known.Assets[Symbol.ES], BrickTicks.From(4));

//        var candles = new List<Candle>();

//        feed.OnCandle += (s, e) => candles.Add(e.Candle);

//        return (feed, candles);
//    }

//    private static void HandleTicks(
//        WickoFeed feed, int tickId, params float[] prices)
//    {
//        foreach (var price in prices)
//        {
//            var dateTime = new DateTime(2022, 10, 10, 9, 30, 0);

//            var tick = new Tick(tickId, TickOn.From(dateTime), price);

//            feed.HandleTick(tick, false);
//        }
//    }

//    private static void CandleShouldBe(Candle candle, int candleId,
//        float open, float high, float low, float close)
//    {
//        candle.CandleId.Should().Be(candleId);
//        candle.Open.Should().Be(open);
//        candle.High.Should().Be(high);
//        candle.Low.Should().Be(low);
//        candle.Close.Should().Be(close);
//    }

//    private static void OpenCandleShouldBe(WickoFeed feed,
//        int candleId, float open, float high, float low, float close)
//    {
//        CandleShouldBe(feed.GetOpenCandle(), candleId, open, high, low, close);
//    }

//    [Fact]
//    public void NoDownAboveLimit()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 9.25f);

//        candles.Count.Should().Be(0);

//        OpenCandleShouldBe(feed, 0, 10.0f, 10.0f, 9.25f, 9.25f);
//    }

//    [Fact]
//    public void NoUpBelowLimit()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 10.75f);

//        candles.Count.Should().Be(0);

//        OpenCandleShouldBe(feed, 0, 10.0f, 10.75f, 10.0f, 10.75f);
//    }

//    [Fact]
//    public void NoDownKissAboveLimit()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 9.0f);

//        candles.Count.Should().Be(0);

//        OpenCandleShouldBe(feed, 0, 10.0f, 10.0f, 9.0f, 9.0f);
//    }

//    [Fact]
//    public void NoUpKissBelowLimit()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 11.0f);

//        candles.Count.Should().Be(0);

//        OpenCandleShouldBe(feed, 0, 10.0f, 11.0f, 10.0f, 11.0f);
//    }

//    [Fact]
//    public void OneDown()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 8.75f);

//        candles.Count.Should().Be(1);

//        CandleShouldBe(candles[0], 0, 10.0f, 10.0f, 9.0f, 9.0f);

//        OpenCandleShouldBe(feed, 1, 9.0f, 9.0f, 8.75f, 8.75f);
//    }

//    [Fact]
//    public void OneUp()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 11.25f);

//        candles.Count.Should().Be(1);

//        CandleShouldBe(candles[0], 0, 10.0f, 11.0f, 10.0f, 11.0f);

//        OpenCandleShouldBe(feed, 1, 11.0f, 11.25f, 11.0f, 11.25f);
//    }

//    [Fact]
//    public void TwoDownThenOneUp()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 7.75f, 10.25f);

//        candles.Count.Should().Be(3);

//        CandleShouldBe(candles[0], 0, 10.0f, 10.0f, 9.0f, 9.0f);
//        CandleShouldBe(candles[1], 1, 9.0f, 9.0f, 8.0f, 8.0f);
//        CandleShouldBe(candles[2], 2, 9.0f, 10.0f, 7.75f, 10.0f);

//        OpenCandleShouldBe(feed, 3, 10.0f, 10.25f, 10.0f, 10.25f);
//    }

//    [Fact]
//    public void TwoUpThenOneDown()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 12.25f, 9.75f);

//        candles.Count.Should().Be(3);

//        CandleShouldBe(candles[0], 0, 10.0f, 11.0f, 10.0f, 11.0f);
//        CandleShouldBe(candles[1], 1, 11.0f, 12.0f, 11.0f, 12.0f);
//        CandleShouldBe(candles[2], 2, 11.0f, 12.25f, 10.0f, 10.0f);

//        OpenCandleShouldBe(feed, 3, 10.0f, 10.0f, 9.75f, 9.75f);
//    }

//    [Fact]
//    public void ThreeDownWithGap()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 6.0f);

//        candles.Count.Should().Be(3);

//        CandleShouldBe(candles[0], 0, 10.0f, 10.0f, 9.0f, 9.0f);
//        CandleShouldBe(candles[1], 1, 9.0f, 9.0f, 8.0f, 8.0f);
//        CandleShouldBe(candles[2], 2, 8.0f, 8.0f, 7.0f, 7.0f);

//        OpenCandleShouldBe(feed, 3, 7.0f, 7.0f, 6.0f, 6.0f);
//    }

//    [Fact]
//    public void ThreeUpWithGap()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 14.0f);

//        candles.Count.Should().Be(3);

//        CandleShouldBe(candles[0], 0, 10.0f, 11.0f, 10.0f, 11.0f);
//        CandleShouldBe(candles[1], 1, 11.0f, 12.0f, 11.0f, 12.0f);
//        CandleShouldBe(candles[2], 2, 12.0f, 13.0f, 12.0f, 13.0f);

//        OpenCandleShouldBe(feed, 3, 13.0f, 14.0f, 13.0f, 14.0f);
//    }

//    [Fact]
//    public void TwoDownThenThreeUpWithGap()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 7.75f);

//        candles.Count.Should().Be(2);

//        CandleShouldBe(candles[0], 0, 10.0f, 10.0f, 9.0f, 9.0f);
//        CandleShouldBe(candles[1], 1, 9.0f, 9.0f, 8.0f, 8.0f);

//        HandleTicks(feed, 2, 12.0f);

//        candles.Count.Should().Be(4);

//        CandleShouldBe(candles[2], 2, 9.0f, 10.0f, 7.75f, 10.0f);
//        CandleShouldBe(candles[3], 3, 10.0f, 11.0f, 10.0f, 11.0f);

//        OpenCandleShouldBe(feed, 4, 11.0f, 12.0f, 11.0f, 12.0f);
//    }

//    [Fact]
//    public void TwoUpThenThreeDownWithGap()
//    {
//        var (feed, candles) = GetFeedAndCandles();

//        HandleTicks(feed, 0, 10.0f, 12.25f);

//        candles.Count.Should().Be(2);

//        CandleShouldBe(candles[0], 0, 10.0f, 11.0f, 10.0f, 11.0f);
//        CandleShouldBe(candles[1], 1, 11.0f, 12.0f, 11.0f, 12.0f);

//        HandleTicks(feed, 2, 8.0f);

//        candles.Count.Should().Be(4);

//        CandleShouldBe(candles[2], 2, 11.0f, 12.25f, 10.0f, 10.0f);
//        CandleShouldBe(candles[3], 3, 10.0f, 10.0f, 9.0f, 9.0f);

//        OpenCandleShouldBe(feed, 4, 9.0f, 9.0f, 8.0f, 8.0f);
//    }
//}