using FluentAssertions;
using SquidEyes.Futures;

namespace SquidEyes.UnitTests;

public class KnownTests
{
    [Fact]
    public void KnownAssets_Should_BeValid_AfterStaticConstruct()
    {
        var symbols = Enum.GetValues<Symbol>();

        Known.Assets.Count.Should().Be(symbols.Length);

        foreach (var symbol in symbols)
        {
            Known.Assets.Should().ContainKey(symbol);
            Known.Assets[symbol].Symbol.Should().Be(symbol);
        }
    }

    [Fact]
    public void KnownTradeDates_Should_BeValid_AfterStaticConstruct()
    {
        var tradeDates = Known.TradeDates;

        tradeDates.Count.Should().Be(2502);

        var holidays = HolidayHelper.GetHolidays();

        foreach (var date in tradeDates.Keys)
        {
            date.IsWeekDay().Should().BeTrue();

            tradeDates[date].AsDateOnly().Should().Be(date);

            holidays.Contains(date).Should().BeFalse();
        }
    }
}