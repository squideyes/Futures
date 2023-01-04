// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class SwingResult
{
    internal SwingResult(TickOn closeOn, double highPlot,
        double highSeries, double lowPlot, double lowSeries)
    {
        CloseOn = closeOn;
        HighPlot = highPlot;
        HighSeries = highSeries;
        LowPlot = lowPlot;
        LowSeries = lowSeries;
    }

    public TickOn CloseOn { get; }
    public double HighPlot { get; }
    public double HighSeries { get; }
    public double LowPlot { get; }
    public double LowSeries { get; }
}