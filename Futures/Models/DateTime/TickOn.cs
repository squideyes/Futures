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

    public int CompareTo(TickOn other) => Value.CompareTo(other.Value);

    public bool Equals(TickOn other) => Value.Equals(other.Value);

    public override bool Equals(object? other) =>
        other is TickOn tickOn && Equals(tickOn);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() =>
        Value.ToString("MM/dd/yyyy HH:mm:ss.fff");

    public TradeDate GetTradeDate(Asset asset)
    {
        asset.MayNot().BeNull();

        var date = DateOnly.FromDateTime(Value);

        if (Value.TimeOfDay > asset.Market.Period.Until)
            date = date.AddDays(1);

        return TradeDate.From(date);
    }

    public static TickOn Parse(string value) =>
        From(DateTime.Parse(value));

    public static bool TryParse(string value, out TickOn tickOn) =>
        Safe.TryGetValue(() => Parse(value), out tickOn);

    public static TickOn From(DateTime value)
    {
        if (!IsValue(value))
            throw new ArgumentOutOfRangeException(nameof(value));

        return new TickOn(value);
    }

    public static TickOn From(int year, int month, int day,
        int hour = 0, int minute = 0, int second = 0, int millisecond = 0)
    {
        return From(new DateTime(
            year, month, day, hour, minute, second, millisecond));
    }

    public static bool IsValue(DateTime value)
    {
        //DateOnly date;

        //if (value.Hour >= 18)
        //    date = DateOnly.FromDateTime(value.Date.AddDays(1));
        //else
        //    date = DateOnly.FromDateTime(value.Date);

        //if (!Known.TradeDates.ContainsKey(date))
        //    return false;

        //var dateTime = date.ToDateTime(TimeOnly.MinValue);

        //return value >= dateTime.AddHours(-6)
        //    && value < dateTime.AddHours(17);

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