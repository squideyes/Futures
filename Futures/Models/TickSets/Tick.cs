namespace SquidEyes.Futures;

public readonly struct Tick : IEquatable<Tick>, IComparable<Tick>
{
    internal Tick(int tickId, TickOn tickOn, float price)
    {
        TickId = tickId;
        TickOn = tickOn;
        Price = price;
    }

    public int TickId { get; }
    public TickOn TickOn { get; }
    public float Price { get; }

    public override string ToString() =>
        $"{Price} ({TickOn}, TickId: {TickId})";

    public string ToCsvString(Asset asset) =>
        $"{TickId},{TickOn},{asset.Format(Price)}";

    public static Tick From(
        Asset asset, int tickId, TickOn tickOn, float price)
    {
        if (tickId < 0)
            throw new ArgumentOutOfRangeException(nameof(tickId));

        if (tickOn.IsDefault())
            throw new ArgumentOutOfRangeException(nameof(tickOn));

        if (!asset.IsPrice(price))
            throw new ArgumentOutOfRangeException(nameof(price));

        return new Tick(tickId, tickOn, price);
    }

    public bool Equals(Tick other) => TickId == other.TickId
        && TickOn == other.TickOn && Price == other.Price;

    public int CompareTo(Tick other) =>
        TickId.CompareTo(other.TickId);

    public override bool Equals(object? other) =>
        other is Tick tick && Equals(tick);

    public override int GetHashCode() =>
        HashCode.Combine(TickId, TickOn, Price);

    public static bool operator ==(Tick left, Tick right) =>
        left.Equals(right);

    public static bool operator !=(Tick left, Tick right) =>
        !(left == right);
}