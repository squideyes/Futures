// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Helpers;
using SquidEyes.Futures.Models;

namespace SquidEyes.UnitTests;

public class ContractTests
{
    [Theory]
    [InlineData("BP", "3,3,3,3")]
    [InlineData("CL", "1,1,1,1,1,1,1,1,1,1,1,1")]
    [InlineData("ES", "3,3,3,3")]
    [InlineData("EU", "3,3,3,3")]
    [InlineData("GC", "2,2,2,2,2,2")]
    [InlineData("JY", "3,3,3,3")]
    [InlineData("NQ", "3,3,3,3")]
    [InlineData("ZB", "3,3,3,3")]
    [InlineData("ZF", "3,3,3,3")]
    [InlineData("ZN", "3,3,3,3")]
    public void GetContractMonths_Should_Return_ExpectedMonths(
        string symbol, string expectedString)
    {
        var asset = KnownAssets.Get(symbol);

        var expected = expectedString.Split(',').Select(int.Parse).ToList();

        var months = asset.Months!.ToList();

        months.Count.Should().Be(expected.Count);

        for (var i = 0; i < months.Count; i++)
            asset.GetContractMonths(months[i]).Should().Be(expected[i]);
    }

    [Theory]
    [InlineData("BP")]
    [InlineData("CL")]
    [InlineData("ES")]
    [InlineData("EU")]
    [InlineData("GC")]
    [InlineData("JY")]
    [InlineData("NQ")]
    [InlineData("ZB")]
    [InlineData("ZF")]
    [InlineData("ZN")]
    public void Contract_Should_Contain_ExpectedTradeDates(string symbol)
    {
        var holidays = HolidayHelper.GetHolidays();

        var asset = KnownAssets.Get(symbol);

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
    [InlineData(Month.January, "01/13/2020")]
    [InlineData(Month.February, "02/17/2020")]
    [InlineData(Month.March, "03/16/2020")]
    [InlineData(Month.April, "04/13/2020")]
    [InlineData(Month.May, "05/18/2020")]
    [InlineData(Month.June, "06/15/2020")]
    [InlineData(Month.July, "07/13/2020")]
    [InlineData(Month.August, "08/17/2020")]
    [InlineData(Month.September, "09/14/2020")]
    [InlineData(Month.October, "10/12/2020")]
    [InlineData(Month.November, "11/16/2020")]
    [InlineData(Month.December, "12/14/2020")]
    public void GetRollDate_Should_Return_ExpectedTradeDate(
        Month month, string dateString)
    {
        var date = Contract.GetRollDate(month, 2020);

        date.DayOfWeek.Should().Be(DayOfWeek.Monday);
        date.Should().Be(DateOnly.Parse(dateString));
    }
}