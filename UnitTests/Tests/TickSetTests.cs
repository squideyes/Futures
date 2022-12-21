using FluentAssertions;
using SquidEyes.Futures;

namespace SquidEyes.UnitTests;

[Collection("Testing")]
public class TickSetTests
{
    private readonly TestingFixture fixture;

    public TickSetTests(TestingFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(Symbol.CL, 98836)]
    [InlineData(Symbol.ES, 210817)]
    [InlineData(Symbol.EU, 26103)]
    [InlineData(Symbol.GC, 39683)]
    [InlineData(Symbol.JY, 16208)]
    [InlineData(Symbol.NQ, 275261)]
    [InlineData(Symbol.TY, 24156)]
    [InlineData(Symbol.US, 16343)]
    public void TickSet_Should_LoadFromStream(
        Symbol symbol, int count)
    {
        fixture.GetTickSet(symbol).Count.Should().Be(count);
    }

    [Fact]
    public void TickSet_Should_RoundTripThroughStream()
    {
        var source = fixture.GetTickSet(Symbol.US);

        TickSet target;

        using (var stream = new MemoryStream())
        {
            source.Save(stream);

            stream.Position = 0;

            target = TickSet.From(stream);
        };

        AssertSourceMatchesTarget(source, target);
    }

    [Theory]
    [InlineData(Symbol.CL, 98836)]
    [InlineData(Symbol.ES, 210817)]
    [InlineData(Symbol.EU, 26103)]
    [InlineData(Symbol.GC, 39683)]
    [InlineData(Symbol.JY, 16208)]
    [InlineData(Symbol.NQ, 275261)]
    [InlineData(Symbol.TY, 24156)]
    [InlineData(Symbol.US, 16343)]
    public void LoadedTickSet_Should_Enumerate(
        Symbol symbol, int total)
    {
        int count = 0;

        foreach(var _ in fixture.GetTickSet(symbol))
            count++;

        count.Should().Be(total);
    }

    [Theory]
    [InlineData(Symbol.CL)]
    [InlineData(Symbol.ES)]
    [InlineData(Symbol.EU)]
    [InlineData(Symbol.GC)]
    [InlineData(Symbol.JY)]
    [InlineData(Symbol.NQ)]
    [InlineData(Symbol.TY)]
    [InlineData(Symbol.US)]
    public void HandCreatedTickSet_Should_Roundtrip(Symbol symbol)
    {
        var source = fixture.GetTickSet(symbol);

        var target = new TickSet(
            source.Source, source.Contract, source.TradeDate);

        foreach (var tick in source)
            target.Add(tick);

        AssertSourceMatchesTarget(source, target);
    }

    [Fact]
    public void GetFullPath_Should_ReturnExpectedPath()
    {
        var tickSet = fixture.GetTickSet(Symbol.NQ);

        tickSet.GetFullPath(@"C:\\Data").Should().Be(
            @"C:\\Data\TickSets\NQ\2022\KB_NQ_20211213_TP_EST.stps");
    }

    private void AssertSourceMatchesTarget(TickSet source, TickSet target)
    {
        target.Contract.Should().Be(source.Contract);
        target.Count.Should().Be(source.Count);
        target.FileName.Should().Be(source.FileName);
        target.MaxTickOn.Should().Be(source.MaxTickOn);
        target.MinTickOn.Should().Be(source.MinTickOn);
        target.Source.Should().Be(source.Source);
        target.TradeDate.Should().Be(source.TradeDate);
        target.ToString().Should().Be(source.ToString());

        for (var i = 0; i < target.Count; i++)
            target[i].Should().Be(source[i]);
    }
}
