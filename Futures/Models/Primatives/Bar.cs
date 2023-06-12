// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Helpers;

namespace SquidEyes.Futures.Models;

public class Bar : IBar
{
    internal Bar(Tick tick, int barId, BarSpec barSpec, Asset asset)
    {
        BarId = barId;
        Open = High = Low = Close = tick.Price;
        Volume = tick.Volume;
        BarSpec = barSpec;
        Asset = asset;

        switch (barSpec.BarKind)
        {
            case BarKind.Minutes:
            case BarKind.Seconds:
                OpenOn = barSpec.GetIntervalOpenOn(tick.TickOn);
                CloseOn = barSpec.GetIntervalCloseOn(tick.TickOn);
                break;
            case BarKind.Range:
            case BarKind.Renko:
            case BarKind.TdRenko:
            case BarKind.TickCount:
                OpenOn = tick.TickOn;
                CloseOn = tick.TickOn;
                break;
        }
    }

    internal void Adjust(Tick tick)
    {
        High = MathF.Max(High, tick.Price);
        Low = MathF.Min(Low, tick.Price);
        Close = tick.Price;
        Volume += tick.Volume;

        switch (BarSpec.BarKind)
        {
            case BarKind.Range:
            case BarKind.Renko:
            case BarKind.TdRenko:
            case BarKind.TickCount:
                CloseOn = tick.TickOn;
                break;
        }
    }

    public int BarId { get; }
    public Asset Asset { get; }
    public DateTime OpenOn { get; }
    public float Open { get; }
    public BarSpec BarSpec { get; }

    public DateTime CloseOn { get; set; }
    public float High { get; set; }
    public float Low { get; set; }
    public float Close { get; set; }
    public int Volume { get; set; }

    public TimeSpan Elapsed => CloseOn - OpenOn;

    public override string ToString()
    {
        var prefix = GetPrefix(OpenOn, CloseOn,
            DateTimeFormat.Default, BarPrefix.OpenAndClose);

        return $"{prefix},{Open},{High},{Low},{Close},{Volume}";
    }

    public string ToString(DateTimeFormat dateTimeFormat, 
        BarPrefix barTimeFormat)
    {
        dateTimeFormat.Must().BeEnumValue();
        barTimeFormat.Must().BeEnumValue();

        var open = Asset.Format(Open);
        var high = Asset.Format(High);
        var low = Asset.Format(Low);
        var close = Asset.Format(Close);

        var prefix = GetPrefix(
            OpenOn, CloseOn, dateTimeFormat, barTimeFormat);

        return $"{prefix},{open},{high},{low},{close},{Volume}";
    }

    private static string GetPrefix(DateTime openOn, DateTime closeOn,
        DateTimeFormat dateTimeFormat, BarPrefix barTimeFormat)
    {
        var openOnText = openOn.ToText(dateTimeFormat);
        var closeOnText = closeOn.ToText(dateTimeFormat);

        return barTimeFormat switch
        {
            BarPrefix.OpenAndClose => $"{openOnText},{closeOnText}",
            BarPrefix.OpenOnly => $"{openOnText}",
            BarPrefix.CloseOnly => $"{closeOnText}",
            _ => throw new ArgumentOutOfRangeException(nameof(barTimeFormat))
        };
    }
}