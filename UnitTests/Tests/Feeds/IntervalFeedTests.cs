//// ********************************************************
//// The use of this source code is licensed under the terms
//// of the MIT License (https://opensource.org/licenses/MIT)
//// ********************************************************

//using SquidEyes.Futures;

//namespace SquidEyes.UnitTests;

//[Collection(nameof(TestingCollection))]
//public class IntervalFeedTests : IClassFixture<TestingFixture>
//{
//    private readonly TestingFixture fixture;

//    public IntervalFeedTests(TestingFixture fixture)
//    {
//        this.fixture = fixture;
//    }

//    [Theory]
//    [InlineData(5, "09:30:04.999", "09:30:05.000")]
//    [InlineData(5, "09:30:05.000", "09:30:10.000")]
//    [InlineData(5, "09:30:09.999", "09:30:10.000")]
//    [InlineData(5, "09:30:10.000", "09:30:15.000")]
//    [InlineData(180, "09:30:00.00", "09:33:00.000")]
//    [InlineData(180, "09:32:59.99", "09:33:00.000")]
//    [InlineData(180, "09:33:00.00", "09:36:00.000")]
//    public void CloseOnReturnsExpectedValue(
//        int seconds, string tickOnString, string target)
//    {
//        const string DATE = "12/05/2022 ";

//        var closeOn = TickOn.Parse(DATE + tickOnString)
//            .ToIntervalOn(seconds);

//        var expected = TickOn.Parse(DATE + target);

//        closeOn.Should().Be(expected);
//    }

//    [Theory]
//    [InlineData(15, 3370)]
//    [InlineData(30, 2206)]
//    [InlineData(60, 1349)]
//    [InlineData(120, 686)]
//    [InlineData(180, 457)]
//    [InlineData(300, 274)]
//    public void FeedFormsExpectedCandles(int seconds, int count)
//    {
//        var candles = new List<Candle>();

//        var tickSet = fixture.GetTickSet(Symbol.ZF);

//        var feed = new IntervalFeed(
//            tickSet.Contract.Asset, tickSet.TradeDate, seconds);

//        feed.OnCandle += (s, e) => candles.Add(e.Candle);

//        int c = 0;

//        foreach (var tick in tickSet)
//            feed.HandleTick(tick, ++c == tickSet.Count);

//        candles.Count.Should().Be(count);

//        for (int i = 1; i < candles.Count; i++)
//        {
//            var candle = candles[i];

//            candle.CloseOn.IsIntervalOn(seconds).Should().BeTrue();
//            candle.CloseOn.Should().BeGreaterThan(candles[i - 1].CloseOn);
//            candle.IsClosed.Should().BeTrue();
//        }
//    }

//    private static void CandleShouldBe(Candle candle,
//        int candleId, float open, float high, float low, float close)
//    {
//        candle.CandleId.Should().Be(candleId);
//        candle.Open.Should().Be(open);
//        candle.High.Should().Be(high);
//        candle.Low.Should().Be(low);
//        candle.Close.Should().Be(close);
//    }
//}