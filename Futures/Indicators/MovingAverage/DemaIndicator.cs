// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class DemaIndicator : BasicIndicatorBase
{
    private readonly EmaIndicator ema1;
    private readonly EmaIndicator ema2;

    public DemaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        ema1 = new EmaIndicator(period);
        ema2 = new EmaIndicator(period);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        var result1 = ema1.AddAndCalc(ohlc).Result;

        var result2 = ema2.AddAndCalc(
            new OHLC(ohlc.CloseOn, (float)result1)).Result;

        var dema = (2.0 * result1) - result2;

        return new BasicResult(ohlc.CloseOn, dema);
    }
}