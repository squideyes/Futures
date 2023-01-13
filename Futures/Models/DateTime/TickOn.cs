// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public readonly struct TickOn : IEquatable<TickOn>, IComparable<TickOn>
{
    private TickOn(DateTime value)
    {
        Value = value;
    }

    private DateTime Value { get; }

    public int Year => Value.Year;
    public int Month => Value.Month;
    public int Day => Value.Day;
    public int Hour => Value.Hour;
    public int Minute => Value.Minute;
    public int Second => Value.Second;
    public int Millisecond => Value.Millisecond;
    public DayOfWeek DayOfWeek => Value.DayOfWeek;

    public DateTime AsDateTime() => Value;

    public TradeDate AsTradeDate()
    {
        var date = DateOnly.FromDateTime(Value);

        if (Value.TimeOfDay > Known.TickOnOffsets.Until)
            date = date.AddDays(1);

        if (!Known.TradeDates.TryGetValue(date, out TradeDate tradeDate))
            throw new InvalidOperationException("TradeDate is invalid!");

        return tradeDate;
    }

    public int CompareTo(TickOn other) => Value.CompareTo(other.Value);

    public bool Equals(TickOn other) => Value.Equals(other.Value);

    public override bool Equals(object? other) =>
        other is TickOn tickOn && Equals(tickOn);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() =>
        Value.ToString("MM/dd/yyyy HH:mm:ss.fff");

    public static bool IsTickOnValue(DateTime value)
    {
        var date = DateOnly.FromDateTime(value);

        if (value.TimeOfDay > Known.TickOnOffsets.Until)
            date = date.AddDays(1);

        if (!Known.TradeDates.TryGetValue(date, out TradeDate tradeDate))
            return false;

        var dateTime = tradeDate.AsDateTime();

        var minValue = dateTime.Add(Known.TickOnOffsets.From);
        var maxValue = dateTime.Add(Known.TickOnOffsets.Until);

        return value >= minValue && value <= maxValue;
    }

    public static TickOn From(DateTime value, TimeSpan offset) =>
        new(value.Add(offset).Must().Be(IsTickOnValue));

    public static TickOn From(DateTime value) =>
        new(value.Must().Be(IsTickOnValue));

    public static TickOn Parse(string value) =>
        From(DateTime.Parse(value));

    public static bool TryParse(string value, out TickOn tickOn)
    {
        tickOn = default;

        if (!DateTime.TryParse(value, out DateTime dateTime))
            return false;

        if (!IsTickOnValue(dateTime))
            return false;

        tickOn = new TickOn(dateTime);

        return true;
    }

    public static bool operator ==(TickOn lhs, TickOn rhs) =>
        lhs.Equals(rhs);

    public static bool operator !=(TickOn lhs, TickOn rhs) =>
        !(lhs == rhs);

    public static bool operator <(TickOn lhs, TickOn rhs) =>
        lhs.CompareTo(rhs) < 0;

    public static bool operator <=(TickOn lhs, TickOn rhs) =>
        lhs.CompareTo(rhs) <= 0;

    public static bool operator >(TickOn lhs, TickOn rhs) =>
        lhs.CompareTo(rhs) > 0;

    public static bool operator >=(TickOn lhs, TickOn rhs) =>
        lhs.CompareTo(rhs) >= 0;
}