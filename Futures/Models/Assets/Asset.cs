// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class Asset : IEquatable<Asset>
{
    private readonly string format;

    internal Asset(Symbol symbol, float oneTick, double tickIdUsd,
        Market market, string description, 
        string months, TimeOnly from, TimeOnly until)
    {
        var digits = oneTick.GetDigits();
        var tickInUsd = Math.Round(tickIdUsd, 2);
        var ticksPerPoint = (int)(1.0f / oneTick);

        Digits = digits;
        Description = description;
        Market = market;
        Months = new SortedSet<Month>(
            months.ToCharArray().Select(v => v.ToMonth()));
        OneTick = oneTick;
        PointInUsd = Math.Round(tickInUsd * ticksPerPoint, 2);
        Stretch = new Stretch(from, until);
        Symbol = symbol;
        TickInUsd = tickInUsd;
        TicksPerPoint = ticksPerPoint;

        format = "F" + digits;
    }

    public Symbol Symbol { get;  }
    public Market? Market { get;  }
    public string? Description { get;  }
    public double TickInUsd { get;  }
    public float OneTick { get;  }
    public int Digits { get;  }
    public int TicksPerPoint { get;  }
    public double PointInUsd { get;  }
    public Stretch Stretch { get; }
    public SortedSet<Month>? Months { get; }

    public bool IsPrice(float value) =>
        value >= OneTick && value == Round(value);

    public float Round(float value)
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