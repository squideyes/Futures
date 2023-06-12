// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

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
}