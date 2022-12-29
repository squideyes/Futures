// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public readonly struct BrickTicks : IEquatable<BrickTicks>
{
    public const int MinValue = 4;
    public const int MaxValue = 100;

    private BrickTicks(int value)
    {
        Value = value;
    }

    private int Value { get; }

    public int AsInt32() => Value;

    public bool Equals(BrickTicks other) => Value.Equals(other.Value);

    public override bool Equals(object? other) =>
        other is BrickTicks brickTicks && Equals(brickTicks);

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value.ToString();

    public static BrickTicks From(int value) =>
        new(value.Must().Be(IsValue));

    public static BrickTicks Parse(string value) => From(int.Parse(value));

    public static bool TryParse(string value, out BrickTicks brickTicks) =>
        Safe.TryGetValue(() => Parse(value), out brickTicks);

    public static bool IsValue(int value) =>
        value >= MinValue && value <= MaxValue;

    public static bool operator ==(BrickTicks lhs, BrickTicks rhs) =>
        lhs.Equals(rhs);

    public static bool operator !=(BrickTicks lhs, BrickTicks rhs) =>
        !(lhs == rhs);
}