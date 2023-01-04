// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public interface IBasicIndicator
{
    BasicResult this[int index] { get; }
    int Count { get; }
    bool IsPrimed { get; }

    BasicResult AddAndCalc(IOHLC candle);
    IEnumerator<BasicResult> GetEnumerator();
}