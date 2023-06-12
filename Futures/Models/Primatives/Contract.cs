// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Futures.Models;
using SquidEyes.Fundamentals;
using System.Text;
using System.Text.RegularExpressions;

namespace SquidEyes.Futures;

public partial class Contract : IEquatable<Contract>, IComparable<Contract>
{
    public static readonly int MinYear = 2020;
    public static readonly int MaxYear = 2029;

    private static readonly Regex parser = Parser();

    public Asset Asset { get; }
    public Month Month { get; }
    public int Year { get; }
    public List<TradeDate> TradeDates { get; } = new();

    internal Contract(Asset asset, Month month, int year)
    {
        Asset = asset;
        Month = month;
        Year = year;

        var contractMonths = asset.GetContractMonths(month);

        var current = GetRollDate(month, year);

        var prior = new DateTime(year, (int)month, 1)
            .AddMonths(-contractMonths)
            .Convert(d => GetRollDate((Month)d.Month, d.Year));

        for (var date = prior; date < current; date = date.AddDays(1))
        {
            if (TradeDateSet.TryGetTradeDate(date, out TradeDate tradeDate))
                TradeDates.Add(tradeDate);
        }
    }

    public bool Equals(Contract? other)
    {
        if (other is null)
            return false;

        return AsTuple().Equals(other.AsTuple());
    }

    public override bool Equals(object? other) =>
        other is Contract contract && Equals(contract);

    override public int GetHashCode() => HashCode.Combine(Asset, Month, Year);

    public int CompareTo(Contract? other) =>
        other is null ? 1 : AsTuple().CompareTo(other.AsTuple());

    public override string ToString() =>
        $"{Asset}{Month.ToLetter()}{Year - 2000}";

    private (Asset, Month, int) AsTuple() => (Asset, Month, Year);

    internal static Contract Parse(string value)
    {
        var match = parser.Match(value);

        if (!match.Success)
            throw new ArgumentOutOfRangeException(nameof(value));

        var asset = AssetSet.From(match.Groups["S"].Value);
        var month = match.Groups["M"].Value[0].ToMonth();
        var year = MinYear + int.Parse(match.Groups["Y"].Value);

        return new Contract(asset, month, year);
    }

    public static bool TryParse(string value, out Contract contract) =>
        Safe.TryGetValue(() => Parse(value), out contract);

    public static bool IsValue(string value)
    {
        var match = parser.Match(value);

        if (!match.Success)
            return false;

        return AssetSet.Contains(match.Groups["S"].Value);
    }

    internal static DateOnly GetRollDate(Month month, int year)
    {
        byte WEEK = 2;

        var date = new DateOnly(year, (int)month, 1);

        date = date.AddDays(-(date.DayOfWeek - DayOfWeek.Thursday));

        date = date.Day > (byte)DayOfWeek.Saturday
            ? date.AddDays(7 * WEEK) : date.AddDays(7 * (WEEK - 1));

        return date.AddDays(4);
    }

    public static bool operator ==(Contract left, Contract right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Contract left, Contract right) => !(left == right);

    public static bool operator <(Contract left, Contract right) => left.CompareTo(right) < 0;

    public static bool operator <=(Contract left, Contract right) => left.CompareTo(right) <= 0;

    public static bool operator >(Contract left, Contract right) => left.CompareTo(right) > 0;

    public static bool operator >=(Contract left, Contract right) => left.CompareTo(right) >= 0;

    [GeneratedRegex(@"^(?<S>[A-Z0-9]{2,4})(?<M>[FGHJKMNQUVXZ])(?<Y>\d)$")]
    private static partial Regex Parser();
}