// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Models;

public class Asset : IEquatable<Asset>
{
    private static readonly Dictionary<string, Asset> assets = new();

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
    }

    public string Description { get; }
    public int Digits { get; }
    public Exchange Exchange { get; }
    public SortedSet<Month> Months { get; }
    public float OneTick { get; }
    public string Symbol { get; }
    public double TickInUsd { get; }
    public int TicksPerPoint { get; }

    public bool IsPrice(float value) =>
        value >= OneTick && value == Round(value);

    public float Round(float value)
    {
        return (float)Math.Round(Math.Round(
            (double)value / OneTick) * OneTick, Digits);
    }

    public string Format(float value) => value.ToString(format);

    public override string ToString() => Symbol.ToString();

    public static Asset[] GetAssets(Exchange? exchange = null)
    {
        return assets.Values.Where(
            a => !exchange.HasValue || a.Exchange == exchange).ToArray();
    }

    public bool Equals(Asset? other) =>
        other is not null && other.Equals(Symbol);

    public override bool Equals(object? other) =>
        other is Asset asset && Equals(asset);

    public override int GetHashCode() => Symbol.GetHashCode();

    public static bool operator ==(Asset left, Asset right)
    {
        if (left is null)
            return right is null;

        return left.Equals(right);
    }

    public static bool operator !=(Asset left, Asset right) =>
        !(left == right);
}