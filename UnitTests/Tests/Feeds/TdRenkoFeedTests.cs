// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

//using SquidEyes.Futures;
//using SquidEyes.UnitTests.Properties;

//namespace SquidEyes.UnitTests;

//[Collection(nameof(TestingCollection))]
//public class TdRenkoFeedTests : IClassFixture<TestingFixture>
//{
//    private readonly TestingFixture fixture;

//    public TdRenkoFeedTests(TestingFixture fixture)
//    {
//        this.fixture = fixture;
//    }

//    [Fact]
//    public void CandlesFormCorrectly()
//    {
//        var candles = new List<Candle>();

//        var tickSet = fixture.GetTickSet(Symbol.NQ);

//        var feed = TdRenkoFeed.Create(
//            tickSet.Contract.Asset, BrickTicks.From(8));

//        feed.OnCandle += (s, e) => candles.Add(e.Candle);

//        var c = 0;

//        foreach (var tick in tickSet)
//            feed.HandleTick(tick, ++c == tickSet.Count);

//        candles.Count.Should().Be(413);

//        var baselines = GetBaselines(tickSet);

//        for (var i = 0; i < candles.Count; i++)
//        {
//            var candle = candles[i];
//            var baseline = baselines[i];

//            candle.CandleId.Should().Be(baseline.CandleId);
//            candle.CloseOn.Should().Be(baseline.CloseOn);
//            candle.Open.Should().Be(baseline.Open);
//            candle.High.Should().Be(baseline.High);
//            candle.Low.Should().Be(baseline.Low);
//            candle.Close.Should().Be(baseline.Close);
//            candle.IsClosed.Should().Be(baseline.IsClosed);
//        }
//    }

//    private static List<Candle> GetBaselines(TickSet tickSet)
//    {
//        var ticks = new List<Candle>();

//        foreach (var fields in new CsvEnumerator(
//            Baselines.TdRenkoFeedBaselines, 6))
//        {
//            var candleId = int.Parse(fields[0]);
//            var closeOn = TickOn.From(DateTime.Parse(fields[1]));
//            var open = float.Parse(fields[2]);
//            var high = float.Parse(fields[3]);
//            var low = float.Parse(fields[4]);
//            var close = float.Parse(fields[5]);

//            ticks.Add(new Candle(tickSet.Contract.Asset,
//                candleId, closeOn, open, high, low, close, true));
//        }

//        return ticks;
//    }
//}