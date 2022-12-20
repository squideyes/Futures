// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class Asset : IEquatable<Asset>
{
    private readonly string format;

    internal Asset(int digits)
    {
        format = "F" + digits;
    }

    public Symbol Symbol { get; internal init; }
    public Market? Market { get; internal init; }
    public string? Description { get; internal init; }
    public double TickInUsd { get; internal init; }
    public float OneTick { get; internal init; }
    public int Digits { get; internal init; }
    public int TicksPerPoint { get; internal init; }
    public double PointInUsd { get; internal init; }
    public Period<TimeOnly> Session { get; internal set; }
    public SortedSet<Month>? Months { get; internal init; }

    public bool IsPrice(float value) =>
        value >= OneTick && value == Round(value);

    internal float Round(float value)
    {
        return (float)Math.Round(Math.Round(
            (double)value / OneTick) * OneTick, Digits);
    }

    public string Format(float value) => value.ToString(format);

    public bool Equals(Asset? other) =>
        other is Asset asset && asset.Symbol.Equals(Symbol);

    public override string ToString() => Symbol.ToString();

    public override bool Equals(object? other) =>
        other is Asset asset && Equals(asset);

    public override int GetHashCode() => Symbol.GetHashCode();

    public static bool operator ==(Asset? lhs, Asset? rhs)
    {
        if (lhs is null)
            return rhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Asset? lhs, Asset? rhs) =>
        !(lhs == rhs);
}