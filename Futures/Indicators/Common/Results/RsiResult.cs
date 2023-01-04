// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class RsiResult
{
    internal RsiResult(
        TickOn closeOn, double @default, double average)
    {
        CloseOn = closeOn;
        Result = @default;
        Average = average;
    }

    public TickOn CloseOn { get; }
    public double Result { get; }
    public double Average { get; }
}