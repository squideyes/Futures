using FluentAssertions;
using SquidEyes.Futures;

namespace SquidEyes.UnitTests;

public class ContractTests
{
    [Theory]
    [InlineData(Symbol.CL)]
    [InlineData(Symbol.ES)]
    [InlineData(Symbol.GC)]
    [InlineData(Symbol.M2K)]
    [InlineData(Symbol.MCL)]
    [InlineData(Symbol.MES)]
    [InlineData(Symbol.MGC)]
    [InlineData(Symbol.MNQ)]
    [InlineData(Symbol.MZB)]
    [InlineData(Symbol.MZN)]
    [InlineData(Symbol.NG)]
    [InlineData(Symbol.NQ)]
    [InlineData(Symbol.QG)]
    [InlineData(Symbol.RTY)]
    [InlineData(Symbol.ZB)]
    [InlineData(Symbol.ZN)]
    public void Contract_Should_Contain_ExpectedTradeDates(Symbol symbol)
    {
        var holidays = HolidayHelper.GetHolidays();

        var asset = Known.Assets[symbol];

        Contract lastContract = null!;

        var fdom = new DateOnly(Contract.MinYear, (int)asset.Months!.First(), 1);

        while (fdom.Year < Contract.MaxYear)
        {
            var contract = new Contract(asset, (Month)fdom.Month, fdom.Year);

            var first = contract.TradeDates.First().AsDateOnly();

            switch (first.DayOfWeek)
            {
                case DayOfWeek.Monday:
                    break;
                case DayOfWeek.Tuesday:
                    holidays.Should().Contain(first.AddDays(-1));
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(first));
            }

            if (lastContract is not null)
            {
                var last = lastContract.TradeDates.Last().AsDateOnly();

                for (var d = last.AddDays(1); d < first.AddDays(-1); d = d.AddDays(1))
                    (d.IsWeekend() || holidays.Contains(d)).Should().BeTrue();
            }

            lastContract = contract;

            fdom = fdom.AddMonths(12 / asset.Months!.Count);
        }
    }

    [Theory]
    [InlineData(2020, Month.January, "01/13/2020")]
    [InlineData(2020, Month.February, "02/17/2020")]
    [InlineData(2020, Month.March, "03/16/2020")]
    [InlineData(2020, Month.April, "04/13/2020")]
    [InlineData(2020, Month.May, "05/18/2020")]
    [InlineData(2020, Month.June, "06/15/2020")]
    [InlineData(2020, Month.July, "07/13/2020")]
    [InlineData(2020, Month.August, "08/17/2020")]
    [InlineData(2020, Month.September, "09/14/2020")]
    [InlineData(2020, Month.October, "10/12/2020")]
    [InlineData(2020, Month.November, "11/16/2020")]
    [InlineData(2020, Month.December, "12/14/2020")]
    public void GetRollDate_Should_Return_ExpectedTradeDate(
        int year, Month month, string dateString)
    {
        var date = Contract.GetRollDate(month, year);

        date.DayOfWeek.Should().Be(DayOfWeek.Monday);
        date.Should().Be(DateOnly.Parse(dateString));
    }
}