// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.UnitTests;

namespace SquidEyes.Praxis.UnitTests;

[Collection(nameof(TestingCollection))]
public class IntervalFeedTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture fixture;

    public IntervalFeedTests(TestingFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(Interval.FifteenSecond, "09:30:14.999", "09:30:15.000")]
    [InlineData(Interval.FifteenSecond, "09:30:15.000", "09:30:30.000")]
    [InlineData(Interval.FifteenSecond, "09:30:29.999", "09:30:30.000")]
    [InlineData(Interval.FifteenSecond, "09:30:30.000", "09:30:45.000")]
    public void OpenOnReturnsExpectedValue(
        Interval interval, string tickOnString, string closeOnString)
    {
        const string PREFIX = "12/05/2022 ";

        var tickOn = TickOn.Parse(PREFIX + tickOnString);
        var closeOn = TickOn.From(tickOn.ToIntervalOn(interval));

        closeOn.Should().Be(TickOn.Parse(PREFIX + closeOnString));
    }

    [Theory]
    //[InlineData(Interval.FifteenSecond, 1557)]
    [InlineData(Interval.ThirtySecond, 780)]
    [InlineData(Interval.OneMinute, 390)]
    [InlineData(Interval.TwoMinute, 195)]
    [InlineData(Interval.ThreeMinute, 130)]
    [InlineData(Interval.FiveMinute, 78)]
    public void FeedFormsExpectedCandles(Interval interval, int count)
    {
        var candles = new List<Candle>();

        var tickSet = fixture.GetTickSet(Symbol.ES);

        var session = Session.From(
            tickSet.Contract.Asset, tickSet.TradeDate);

        var feed = new IntervalFeed(
            tickSet.Contract.Asset, session, interval);

        feed.OnCandle += (s, e) => candles.Add(e.Candle);

        int c = 0;

        foreach (var tick in tickSet)
            feed.HandleTick(tick, ++c == tickSet.Count);

        candles.Count.Should().Be(count);

        for (int i = 1; i < candles.Count; i++)
        {
            var candle = candles[i];

            if (i == candles.Count - 1)
            {
                candle.CloseOn.Should().Be(tickSet.Last().TickOn);

                candle.IsClosed.Should().BeFalse();
            }
            else
            {
                var lastCloseOn = TickOn.From(candles[i - 1].CloseOn
                    .AsDateTime().AddSeconds((int)interval));

                candle.CloseOn.Should().Be(lastCloseOn);

                candle.IsClosed.Should().BeTrue();
            }
        }
    }

    private (IntervalFeed, List<Candle>) GetFeedAndCandles(
        Asset asset, Session session)
    {
        var feed = new IntervalFeed(
            asset, session, Interval.FifteenSecond);

        var candles = new List<Candle>();

        feed.OnCandle += (s, e) => candles.Add(e.Candle);

        return (feed, candles);
    }

    private static void CandleShouldBe(Candle candle, int candleId,
        float open, float high, float low, float close)
    {
        candle.CandleId.Should().Be(candleId);
        candle.Open.Should().Be(open);
        candle.High.Should().Be(high);
        candle.Low.Should().Be(low);
        candle.Close.Should().Be(close);
    }
}