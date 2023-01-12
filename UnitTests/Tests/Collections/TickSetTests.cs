// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.Testing;

namespace SquidEyes.UnitTests;

[Collection("TestingCollection")]
public class TickSetTests
{
    private readonly TestingFixture fixture;

    public TickSetTests(TestingFixture fixture)
    {
        this.fixture = fixture;
    }

    [Theory]
    [InlineData(Symbol.GC, 47511)]
    [InlineData(Symbol.NQ, 102630)]
    [InlineData(Symbol.ZF, 21953)]
    public void TickSet_Should_LoadFromStream(
        Symbol symbol, int count)
    {
        fixture.GetTickSet(symbol).Count.Should().Be(count);
    }

    [Theory]
    [InlineData(Symbol.GC)]
    [InlineData(Symbol.NQ)]
    [InlineData(Symbol.ZF)]
    public void TickSet_Should_RoundTripThroughStream(Symbol symbol)
    {
        var source = fixture.GetTickSet(symbol);

        TickSet target;

        using (var stream = new MemoryStream())
        {
            source.Save(stream);

            stream.Position = 0;

            target = TickSet.From(stream);
        };

        TestingHelper.AssertSourceMatchesTarget(source, target);
    }

    [Theory]
    [InlineData(Symbol.GC, 47511)]
    [InlineData(Symbol.NQ, 102630)]
    [InlineData(Symbol.ZF, 21953)]
    public void LoadedTickSet_Should_Enumerate(
        Symbol symbol, int total)
    {
        int count = 0;

        foreach(var _ in fixture.GetTickSet(symbol))
            count++;

        count.Should().Be(total);
    }

    [Theory]
    [InlineData(Symbol.GC)]
    [InlineData(Symbol.NQ)]
    [InlineData(Symbol.ZF)]
    public void HandCreatedTickSet_Should_Roundtrip(Symbol symbol)
    {
        var source = fixture.GetTickSet(symbol);

        var target = new TickSet(
            source.Source, source.Contract, source.TradeDate);

        foreach (var tick in source)
            target.Add(tick);

        TestingHelper.AssertSourceMatchesTarget(source, target);
    }

    [Theory]
    [InlineData(Symbol.GC)]
    [InlineData(Symbol.NQ)]
    [InlineData(Symbol.ZF)]
    public void GetFullPath_Should_ReturnExpectedPath(Symbol symbol)
    {
        var tickSet = fixture.GetTickSet(symbol);

        tickSet.GetFullPath(@"C:\\Data").Should().Be(
            $@"C:\\Data\Kibot\{symbol}\2020\KB_{symbol}_20191216_TP_EST.stps");
    }
}