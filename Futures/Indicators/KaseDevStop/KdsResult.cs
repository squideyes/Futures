// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class KdsResult
{
    public KdsResult()
    {
        BarKind = KdsBarKind.None;
    }

    public KdsTrend Trend { get; internal set; }
    public double WarningLine { get; internal set; }
    public double StopDot1 { get; internal set; }
    public double StopDot2 { get; internal set; }
    public double StopDot3 { get; internal set; }
    public KdsBarKind BarKind { get; internal set; }
    public KdsDotKind WarningKind { get; internal set; }

    internal string ToCsvString() =>
        $"{Trend},{WarningLine},{StopDot1},{StopDot2},{StopDot3},{BarKind},{WarningKind}";

    internal static KdsResult Parse(string csv)
    {
        var fields = csv.Split(',');

        fields.Length.Must().Be(7);

        return new KdsResult()
        {
            Trend = Enum.Parse<KdsTrend>(fields[0], true),
            WarningLine = double.Parse(fields[1]),
            StopDot1 = double.Parse(fields[2]),
            StopDot2 = double.Parse(fields[3]),
            StopDot3 = double.Parse(fields[4]),
            BarKind = Enum.Parse<KdsBarKind>(fields[5], true),
            WarningKind = Enum.Parse<KdsDotKind>(fields[6], true)
        };
    }
}