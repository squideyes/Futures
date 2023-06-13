// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Helpers;

namespace SquidEyes.Futures.Models;

public readonly struct Tick
{
    internal Tick(DateTime tickOn, float price, int volume)
    {
        TickOn = tickOn;
        Price = price;
        Volume = volume;
    }

    public DateTime TickOn { get;  }
    public float Price { get;  }
    public int Volume { get; }

    public static Tick From(DateTime tickOn, float price, int volume)
    {
        tickOn.Must().Be(v => v.IsTickOn());
        price.Must().BePositive();
        volume.Must().BePositiveOrZero();

        return new Tick(tickOn, price, volume);
    }
}