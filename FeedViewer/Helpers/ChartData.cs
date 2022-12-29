// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.FeedViewer;

public class ChartData
{
    public DateTime CloseOn { get; set; }
    public double Open { get; set; }
    public double High { get; set; }
    public double Low { get; set; }
    public double Close { get; set; }

    public string ToCsvString() =>
        $"{CloseOn:MM/dd/yyyy HH:mm:ss.fff},{Open},{High},{Low},{Close}";
}