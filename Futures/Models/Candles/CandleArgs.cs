// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class CandleArgs : EventArgs
{
    public CandleArgs(Candle candle, Tick tick)
    {
        Candle = candle.MayNot().BeNull();
        Tick = tick.MayNot().BeDefault();
    }

    public Candle Candle { get; }
    public Tick Tick { get; }
}