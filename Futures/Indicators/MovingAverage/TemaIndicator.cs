// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class TemaIndicator : BasicIndicatorBase
{
    private readonly EmaIndicator ema1;
    private readonly EmaIndicator ema2;
    private readonly EmaIndicator ema3;

    public TemaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        ema1 = new EmaIndicator(period);
        ema2 = new EmaIndicator(period);
        ema3 = new EmaIndicator(period);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        var v1 = ema1.AddAndCalc(ohlc).Result;
        var r1 = new BasicResult(ohlc.CloseOn, v1);

        var v2 = ema2.AddAndCalc(new OHLC(r1.CloseOn, (float)r1.Result)).Result;
        var r2 = new BasicResult(ohlc.CloseOn, v2);

        var v3 = ema3.AddAndCalc(new OHLC(r1.CloseOn, (float)r2.Result)).Result;
        var tema = (3 * v1) - (3 * v2) + v3;

        return new BasicResult(ohlc.CloseOn, tema);
    }
}