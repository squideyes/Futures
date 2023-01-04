// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class AtrIndicator : BasicIndicatorBase
{
    private int index = 0;
    private double lastValue = 0.0;
    private IOHLC lastOhlc = null!;

    public AtrIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        double high0 = ohlc.High;
        double low0 = ohlc.Low;

        BasicResult result;

        if (index++ == 0)
        {
            lastOhlc = ohlc;
            lastValue = high0 - low0;

            result = new BasicResult(ohlc.CloseOn, lastValue);
        }
        else
        {
            var close1 = lastOhlc.Close;

            var trueRange = Math.Max(Math.Abs(low0 - close1),
                Math.Max(high0 - low0, Math.Abs(high0 - close1)));

            var atr = ((Math.Min(index, Period) - 1) *
                lastValue + trueRange) / Math.Min(index, Period);

            lastOhlc = ohlc;
            lastValue = atr;

            result = new BasicResult(ohlc.CloseOn, atr);
        }

        return result;
    }
}