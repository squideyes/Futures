namespace SquidEyes.Futures;

public readonly struct TradeDate : IEquatable<TradeDate>, IComparable<TradeDate>
{
    public static readonly DateOnly MinValue = new(2019, 12, 16);
    public static readonly DateOnly MaxValue = new(2029, 12, 14);

    internal TradeDate(DateOnly value)
    {
        Value = value;
    }

    public int Year => Value.Year;
    public int Month => Value.Month;
    public int Day => Value.Day;
    public DayOfWeek DayOfWeek => Value.DayOfWeek;

    private DateOnly Value { get; }

    public DateOnly AsDateOnly() => Value;

    public DateTime AsDateTime() => Value.ToDateTime(TimeOnly.MinValue);

    public bool Equals(TradeDate other) => Value.Equals(other.Value);

    public override bool Equals(object? other) =>
        other is TradeDate tradeDate && Equals(tradeDate);

    public int CompareTo(TradeDate other) => Value.CompareTo(other.Value);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString("MM/dd/yyyy");

    public string ToString(string format) => Value.ToString(format);

    public static TradeDate From(DateOnly value)
    {
        if (!IsValue(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        return new(value);
    }

    public static TradeDate From(int year, int month, int day) =>
        From(new DateOnly(year, month, day));

    public static bool IsValue(DateOnly value) => true;
        //Known.TradeDates.ContainsKey(value);

    public static TradeDate Parse(string value) =>
        From(DateOnly.Parse(value));

    public static bool TryParse(string value, out TradeDate tradeDate) =>
        Safe.TryGetValue(() => Parse(value), out tradeDate);

    public static bool operator ==(TradeDate lhs, TradeDate rhs) =>
        lhs.Equals(rhs);

    public static bool operator !=(TradeDate lhs, TradeDate rhs) =>
        !(lhs == rhs);

    public static bool operator <(TradeDate lhs, TradeDate rhs) =>
        lhs.CompareTo(rhs) < 0;

    public static bool operator <=(TradeDate lhs, TradeDate rhs) =>
        lhs.CompareTo(rhs) <= 0;

    public static bool operator >(TradeDate lhs, TradeDate rhs) =>
        lhs.CompareTo(rhs) > 0;

    public static bool operator >=(TradeDate lhs, TradeDate rhs) =>
        lhs.CompareTo(rhs) >= 0;
}