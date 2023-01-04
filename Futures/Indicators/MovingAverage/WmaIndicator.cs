// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class WmaIndicator : BasicIndicatorBase
{
    private readonly SlidingBuffer<float> buffer;

    private int index = 0;
    private double priorSum;
    private double priorWsum;

    public WmaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        buffer = new SlidingBuffer<float>(Period + 1);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        buffer.Add(ohlc.Close);

        var factor = Math.Min(index + 1, Period);

        var wsum = priorWsum -
            (index >= Period ? priorSum : 0.0) + factor * ohlc.Close;

        var sum = priorSum + ohlc.Close -
            (index >= Period ? buffer[0] : 0.0);

        var wma = wsum / (0.5 * factor * (factor + 1));

        index++;

        priorWsum = wsum;
        priorSum = sum;

        return new BasicResult(ohlc.CloseOn, wma);
    }
}