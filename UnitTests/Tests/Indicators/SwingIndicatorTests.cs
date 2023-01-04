// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.UnitTests.Properties;

namespace SquidEyes.UnitTests;

[Collection(nameof(TestingCollection))]
public class SwingIndicatorTests : IClassFixture<TestingFixture>
{
    private class Baseline
    {
        public double HighPlot { get; set; }
        public double HighSeries { get; set; }
        public double LowPlot { get; set; }
        public double LowSeries { get; set; }
    }   
    
    private readonly TestingFixture fixture;

    public SwingIndicatorTests(TestingFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void BaselineTest()
    {
        const int SWING_STRENGTH = 1;

        var indicator = new SwingIndicator(SWING_STRENGTH);

        var candles = fixture.GetCandleSet(Symbol.NQ);

        foreach (var candle in candles)
            indicator.AddAndCalc(candle);

        var results = indicator.Reverse().ToArray();

        var baselines = GetBaselines();

        for (var i = 0; i < results.Length; i++)
        {
            var result = results[i];
            var baseline = baselines[i];

            result.HighPlot.Should().Be(baseline.HighPlot);
            result.HighSeries.Should().Be(baseline.HighSeries);
            result.LowPlot.Should().Be(baseline.LowPlot);
            result.LowSeries.Should().Be(baseline.LowSeries);
        }
    }

    private static List<Baseline> GetBaselines()
    {
        var baselines = new List<Baseline>();

        foreach (var fields in new CsvEnumerator(
            Baselines.SwingIndicatorBaselines, 4))
        {
            baselines.Add(new Baseline()
            {
                HighPlot = double.Parse(fields[0]),
                HighSeries = double.Parse(fields[1]),
                LowPlot = double.Parse(fields[2]),
                LowSeries = double.Parse(fields[3]),
            });
        }

        return baselines;
    }
}