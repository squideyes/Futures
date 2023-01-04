// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class EmaIndicator : BasicIndicatorBase
{
    private readonly double constant1;
    private readonly double constant2;
    private double? lastEma;

    public EmaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        constant1 = 2.0 / (1 + Period);
        constant2 = 1.0 - (2.0 / (1 + Period));
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        var ema = !lastEma.HasValue ? ohlc.Close :
            ohlc.Close * constant1 + constant2 * lastEma;

        lastEma = ema;

        return new BasicResult(ohlc.CloseOn, ema.Value);
    }
}