// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Models;

public class Asset : IEquatable<Asset>, IComparable<Asset>
{
    private readonly Dictionary<TradeDate, Contract> contractByTradeDate = new();

    private readonly string format;

    internal Asset(string symbol, Exchange exchange, string months,
        float oneTick, double tickInUsd, string description)
    {
        Description = description;
        Digits = oneTick.GetDigits();
        Exchange = exchange;
        Months = months.ToMonths();
        OneTick = oneTick;
        Symbol = symbol;
        TickInUsd = tickInUsd;
        TicksPerPoint = (int)(1.0f / oneTick);

        format = "F" + Digits;

        Contracts = GetContracts();

        foreach (var contract in Contracts)
        {
            foreach (var tradeDate in contract.TradeDates)
                contractByTradeDate.Add(tradeDate, contract);
        }
    }

    public string Description { get; }
    public int Digits { get; }
    public Exchange Exchange { get; }
    public SortedSet<Month> Months { get; }
    public float OneTick { get; }
    public string Symbol { get; }
    public double TickInUsd { get; }
    public int TicksPerPoint { get; }
    public Contract[] Contracts { get; }

    public Contract GetContract(TradeDate tradeDate) =>
        contractByTradeDate[tradeDate];

    public bool IsPrice(float value) =>
        value >= OneTick && value == Round(value);

    public float Round(float value)
    {
        return (float)Math.Round(Math.Round(
            (double)value / OneTick) * OneTick, Digits);
    }

    public string Format(float value) => value.ToString(format);

    public override string ToString() => Symbol.ToString();

    public bool Equals(Asset? other) =>
        other is not null && other.Equals(Symbol);

    public override bool Equals(object? other) =>
        other is Asset asset && Equals(asset);

    public override int GetHashCode() => Symbol.GetHashCode();

    public int CompareTo(Asset? other) =>
        Symbol.CompareTo(other!.Symbol);

    Contract[] GetContracts()
    {
        var contracts = new List<Contract>();

        for (int year = Contract.MinYear;
            year <= Contract.MaxYear; year++)
        {
            foreach (var month in Months!)
                contracts.Add(new Contract(this, month, year));
        }

        return contracts.ToArray();
    }

    public static bool operator ==(Asset left, Asset right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Asset left, Asset right) =>
        !(left == right);
}