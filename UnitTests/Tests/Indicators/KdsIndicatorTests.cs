//// ********************************************************
//// The use of this source code is licensed under the terms
//// of the MIT License (https://opensource.org/licenses/MIT)
//// ********************************************************

//using SquidEyes.Futures;
//using SquidEyes.UnitTests.Properties;

//namespace SquidEyes.UnitTests;

//[Collection(nameof(TestingCollection))]
//public class KdsIndicatorTests : IClassFixture<TestingFixture>
//{
//    private class Baseline
//    {
//        public double WarningLine { get; set; }
//        public double StopDot1 { get; set; }
//        public double StopDot2 { get; set; }
//        public double StopDot3 { get; set; }
//        public KdsTrend Trend { get; set; }
//        public KdsBarKind BarKind { get; set; }
//    }

//    private readonly TestingFixture fixture;

//    public KdsIndicatorTests(TestingFixture fixture)
//    {
//        this.fixture = fixture;
//    }

//    [Fact]
//    public void BaselineTest()
//    {
//        var settings = new KdsSettings()
//        {
//            AnchorMode = KdsAnchorMode.HighLow,
//            TrailingStop = true,
//            FastMA = MaKind.SMA,
//            SlowMA = MaKind.SMA,
//            FastPeriod = 14,
//            SlowPeriod = 21,
//            AvgTbtrPeriod = 30,
//            AvgTbtrMultiplier = 1.0,
//            StdDevPeriod = 30,
//            StdDevMultiplier1 = 1.0,
//            StdDevMultiplier2 = 2.6,
//            StdDevMultiplier3 = 3.3
//        };

//        var candles = fixture.GetCandleSet(Symbol.NQ);

//        var indicator = new KdsIndicator(
//            candles.Contract.Asset, settings);

//        var actuals = new List<KdsResult>();

//        foreach (var candle in candles)
//            actuals.Add(indicator.AddAndCalc(candle));

//        var baselines = GetBaselines();

//        actuals.Count.Should().Be(baselines.Count);

//        for (var i = 0; i < actuals.Count; i++)
//        {
//            var a = actuals[i];
//            var b = baselines[i];

//            a.WarningLine.Should().Be(b.WarningLine);
//            a.StopDot1.Approximates(b.StopDot1).Should().BeTrue();
//            a.StopDot2.Approximates(b.StopDot2).Should().BeTrue();
//            a.StopDot3.Approximates(b.StopDot3).Should().BeTrue();
//            a.Trend.Should().Be(b.Trend);
//            a.BarKind.Should().Be(b.BarKind);
//        }
//    }

//    private static List<KdsResult> GetBaselines()
//    {
//        var results = new List<KdsResult>();

//        var reader = new StringReader(Baselines.KdsIndicatorBaselines);

//        string line;

//        while ((line = reader.ReadLine()!) != null)
//            results.Add(KdsResult.Parse(line));

//        return results;
//    }
//}