// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;

namespace SquidEyes.Futures;

public class BollingerBandIndicator : IEnumerable<ChannelResult>
{
    private readonly SlidingBuffer<ChannelResult> results;

    private readonly SmaIndicator smaIndicator;
    private readonly StdDevIndicator stdDevIndicator;
    private readonly double stdDevFactor;

    public BollingerBandIndicator(
        int period, double stdDevFactor, int bufferSize = 100)
    {
        this.stdDevFactor = stdDevFactor.Must().BePositive();

        smaIndicator = new SmaIndicator(period);

        stdDevIndicator = new StdDevIndicator(period);

        results = new SlidingBuffer<ChannelResult>(bufferSize, true);
    }

    public ChannelResult this[int index] => results[index];

    public bool IsPrimed => results.IsPrimed;

    public ChannelResult AddAndCalc(Candle candle)
    {
        candle.MayNot().BeNull();

        var smaValue = smaIndicator.AddAndCalc(candle).Result;

        var stdDevValue = stdDevIndicator.AddAndCalc(candle).Result;

        var delta = stdDevFactor * stdDevValue;

        var result = new ChannelResult(candle.CloseOn, 
            smaValue + delta, smaValue, smaValue - delta);

        results.Add(result);

        return result;
    }

    public IEnumerator<ChannelResult> GetEnumerator()
    {
        foreach (var result in results)
            yield return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}