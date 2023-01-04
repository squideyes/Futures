// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class SmaIndicator : BasicIndicatorBase
{
    private readonly SlidingBuffer<double> buffer;

    private int index = 0;
    private double sum = 0;
    private double priorSum = 0;

    public SmaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        buffer = new SlidingBuffer<double>(Period + 1);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        buffer.Add(ohlc.Close);

        sum = priorSum + ohlc.Close - (index >= Period ? buffer[0] : 0);

        var sma = sum / (index < Period ? index + 1 : Period);

        priorSum = sum;

        index++;

        return new BasicResult(ohlc.CloseOn, sma);
    }
}