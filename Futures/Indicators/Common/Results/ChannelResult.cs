// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class ChannelResult 
{
    internal ChannelResult(TickOn closeOn, 
        double upper, double middle, double lower)
    {
        CloseOn = closeOn;
        Upper = upper;
        Middle = middle;
        Lower = lower;
    }

    public TickOn CloseOn { get; }
    public double Upper { get; }
    public double Middle { get;}
    public double Lower { get; }
}