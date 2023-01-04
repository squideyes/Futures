// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class StdDevIndicator : BasicIndicatorBase
{
    private readonly SlidingBuffer<double> prices;
    private readonly SlidingBuffer<double> sumSeries;

    private int index = 0;

    public StdDevIndicator(int period, int bufferSize = 100)
        : base(period, bufferSize)
    {
        prices = new SlidingBuffer<double>(period + 1, true);

        sumSeries = new SlidingBuffer<double>(period + 1, true);
    }

    protected override BasicResult GetBasicResult(IOHLC ohlc)
    {
        prices.Add(ohlc.Close);

        sumSeries.Add(0.0);

        BasicResult result;

        if (index < 1)
        {
            sumSeries.Update(ohlc.Close);

            index++;

            result = new BasicResult(ohlc.CloseOn, 0.0);
        }
        else
        {
            sumSeries.Update(ohlc.Close + sumSeries[1] -
                (index >= Period ? prices[Period] : 0.0));

            var average = sumSeries[0] / Math.Min(index + 1, Period);

            var sum = 0.0;

            for (var barsBack = Math.Min(index, Period - 1); barsBack >= 0; barsBack--)
                sum += (prices[barsBack] - average) * (prices[barsBack] - average);

            result = new BasicResult(ohlc.CloseOn,
                Math.Sqrt(sum / Math.Min(index + 1, Period)));

            index++;
        }

        return result;
    }
}