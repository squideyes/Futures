// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class BasicResult
{
    internal BasicResult(TickOn closeOn, double result)
    {
        CloseOn = closeOn;
        Result = result;
    }

    public TickOn CloseOn { get; }
    public double Result { get; }
}