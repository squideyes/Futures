// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.UnitTests;

using SquidEyes.UnitTests.Properties;

namespace SquidEyes.Trading;

[Collection(nameof(TestingCollection))]
public class BasicIndicatorTests : IClassFixture<TestingFixture>
{
    private readonly TestingFixture fixture;

    public BasicIndicatorTests(TestingFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public void AtrIndicatorBaselineTest()
    {
        RunAndValidate(new AtrIndicator(10, 1000),
            Baselines.AtrIndicatorResults);
    }

    [Fact]
    public void DemaIndicatorBaselineTest()
    {
        RunAndValidate(new DemaIndicator(10, 1000),
            Baselines.DemaIndicatorResults);
    }

    [Fact]
    public void EmaIndicatorBaselineTest()
    {
        RunAndValidate(new EmaIndicator(10, 1000),
            Baselines.EmaIndicatorResults);
    }

    [Fact]
    public void KamaIndicatorBaselineTest()
    {
        RunAndValidate(new KamaIndicator(10, 5, 15, 1000),
            Baselines.KamaIndicatorResults);
    }

    [Fact]
    public void SmaIndicatorBaselineTest()
    {
        RunAndValidate(new SmaIndicator(10, 1000),
            Baselines.SmaIndicatorResults);
    }

    [Fact]
    public void SmmaIndicatorBaselineTest()
    {
        RunAndValidate(new SmmaIndicator(10, 1000),
            Baselines.SmmaIndicatorResults);
    }

    [Fact]
    public void StdDevIndicatorBaselineTest()
    {
        RunAndValidate(new StdDevIndicator(10, 1000),
            Baselines.StdDevIndicatorResults);
    }

    [Fact]
    public void TemaIndicatorBaselineTest()
    {
        RunAndValidate(new TemaIndicator(10, 1000),
            Baselines.TemaIndicatorResults);
    }

    [Fact]
    public void WmaIndicatorBaselineTest()
    {
        RunAndValidate(new WmaIndicator(10, 1000),
            Baselines.WmaIndicatorResults);
    }

    private void RunAndValidate(IBasicIndicator indicator, string csv)
    {
        var candles = fixture.GetCandleSet(Symbol.NQ);

        for (var i = candles.Count - 1; i >= 0; i--)
            indicator.AddAndCalc(candles[i]);

        var index = 0;

        foreach (var fields in new CsvEnumerator(csv, 2))
        {
            var closeOn = TickOn.Parse(fields[0]);
            var result = double.Parse(fields[1]);

            var source = indicator[index++];

            source.CloseOn.Should().Be(closeOn);
            source.Result.Should().Be(result);
        }

        indicator.Count.Should().Be(index);
    }
}