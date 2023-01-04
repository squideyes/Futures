// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class SmmaIndicator : BasicIndicatorBase
{
    private readonly SlidingBuffer<float> buffer;

    private int index = 0;
    private double lastSmma = 0;
    private double sum = 0;
    private double prevsum = 0;
    private double prevsmma = 0;

    public SmmaIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        buffer = new SlidingBuffer<float>(period);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        buffer.Add(ohlc.Close);

        double smma;

        if (index++ <= Period)
        {
            sum = buffer.Sum();

            lastSmma = sum / Period;

            smma = lastSmma;
        }
        else
        {
            prevsum = sum;

            prevsmma = lastSmma;

            smma = (prevsum - prevsmma + ohlc.Close) / Period;

            sum = prevsum - prevsmma + ohlc.Close;

            lastSmma = (sum - prevsmma + ohlc.Close) / Period;
        }

        return new BasicResult(ohlc.CloseOn, smma);
    }
}