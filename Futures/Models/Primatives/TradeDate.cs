// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures.Models;

public readonly struct TradeDate : IEquatable<TradeDate>, IComparable<TradeDate>
{
    public static readonly DateOnly MinValue = new(2019, 12, 16);
    public static readonly DateOnly MaxValue = new(2029, 12, 14);

    public static readonly TimeSpan MinOffset = TimeSpan.FromHours(-6);
    public static readonly TimeSpan MaxOffset = new(0, 16, 59, 59, 999);

    internal TradeDate(DateOnly value)
    {
        Value = value;
    }

    public int Month => Value.Month;
    public int Day => Value.Day;
    public int Year => Value.Year;
    public int DayNumber => Value.DayNumber;
    public int DayOfYear => Value.DayOfYear;
    public DayOfWeek DayOfWeek => Value.DayOfWeek;

    private DateOnly Value { get; }

    public DateTime MinTickOn => 
        Value.ToDateTime(TimeOnly.MinValue).Add(MinOffset);

    public DateTime MaxTickOn => 
        Value.ToDateTime(TimeOnly.MinValue).Add(MaxOffset);

    public bool IsTickOn(DateTime value) =>
        value >= MinTickOn && value <= MaxTickOn;

    public DateOnly AsDateOnly() => Value;

    public DateTime AsDateTime() => Value.ToDateTime(TimeOnly.MinValue);

    public bool Equals(TradeDate other) => Value.Equals(other.Value);

    public override bool Equals(object? other) =>
        other is TradeDate tradeDate && Equals(tradeDate);

    public int CompareTo(TradeDate other) => Value.CompareTo(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString("MM/dd/yyyy");

    public static bool operator ==(TradeDate left, TradeDate right) =>
        left.Equals(right);

    public static bool operator !=(TradeDate left, TradeDate right) =>
        !(left == right);

    public static bool operator <(TradeDate left, TradeDate right) =>
        left.CompareTo(right) < 0;

    public static bool operator <=(TradeDate left, TradeDate right) =>
        left.CompareTo(right) <= 0;

    public static bool operator >(TradeDate left, TradeDate right) =>
        left.CompareTo(right) > 0;

    public static bool operator >=(TradeDate left, TradeDate right) =>
        left.CompareTo(right) >= 0;
}