// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class KamaIndicator : BasicIndicatorBase
{
    private readonly double fastCF;
    private readonly double slowCF;
    private readonly int period;
    private readonly SlidingBuffer<double> diffs;
    private readonly SlidingBuffer<double> values;

    private double lastResult = 0;

    private int index = 0;

    public KamaIndicator(int period, int fast, int slow, int bufferSize = 100)
        : base(period, bufferSize)
    {
        fast.Must().BeBetween(5, 99);
        slow.Must().BeBetween(5, 99);

        fastCF = 2.0 / (fast + 1);
        slowCF = 2.0 / (slow + 1);
        diffs = new SlidingBuffer<double>(Period + 1, true);
        values = new SlidingBuffer<double>(Period + 1, true);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        var data = new BasicResult(ohlc.CloseOn, ohlc.Close);

        BasicResult UpdateIndexAndLastResultThenGetResult(double value)
        {
            index++;

            return new BasicResult(ohlc.CloseOn, lastResult = value);
        }

        values.Add(data.Result);

        diffs.Add(index > 0 ? Math.Abs(data.Result - values[1]) : data.Result);

        if (index < period)
            return UpdateIndexAndLastResultThenGetResult(data.Result);

        var signal = Math.Abs(data.Result - values[period]);

        var noise = diffs.Take(period).Sum();

        if (noise == 0.0)
            return UpdateIndexAndLastResultThenGetResult(data.Result);

        var value = lastResult + Math.Pow(signal / noise *
            (fastCF - slowCF) + slowCF, 2) * (data.Result - lastResult);

        return UpdateIndexAndLastResultThenGetResult(value);
    }
}