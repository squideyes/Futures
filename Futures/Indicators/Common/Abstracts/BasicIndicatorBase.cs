// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;

namespace SquidEyes.Futures;

public abstract class BasicIndicatorBase : IEnumerable<BasicResult>, IBasicIndicator
{
    private readonly SlidingBuffer<BasicResult> results;

    public BasicIndicatorBase(int period, int bufferSize)
    {
        Period = period.Must().BeBetween(1, 99);

        results = new SlidingBuffer<BasicResult>(bufferSize, true);
    }

    protected int Period { get; }

    public BasicResult this[int index] => results[index];

    public int Count => results.Count;

    public bool IsPrimed => results.IsPrimed;

    public BasicResult AddAndCalc(IOHLC ohlc)
    {
        ohlc.MayNot().BeNull();

        var result = GetBasicResult(ohlc);

        results.Add(result);

        return result;
    }

    protected abstract BasicResult GetBasicResult(IOHLC ohlc);

    public IEnumerator<BasicResult> GetEnumerator()
    {
        foreach (var result in results)
            yield return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}