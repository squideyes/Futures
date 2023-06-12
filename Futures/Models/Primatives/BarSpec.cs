// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Models;

public readonly struct BarSpec
{
    private BarSpec(BarKind barKind, int barSize)
    {
        BarKind = barKind;
        BarSize = barSize;
    }

    public BarKind BarKind { get; }
    public int BarSize { get; }

    internal DateTime GetIntervalOpenOn(DateTime tickOn) =>
        new(tickOn.Ticks - (tickOn.Ticks % GetInterval().Ticks));

    internal DateTime GetIntervalCloseOn(DateTime tickOn)=>
        GetIntervalOpenOn(tickOn).Add(GetInterval()).AddMilliseconds(-1);

    public override string ToString() => 
        $"{BarKind.ToCode()}{BarSize:0000}";

    private TimeSpan GetInterval()
    {
        return BarKind switch
        {
            BarKind.Seconds => TimeSpan.FromSeconds(BarSize),
            BarKind.Minutes => TimeSpan.FromMinutes(BarSize),
            _ => throw new InvalidOperationException()
        };
    }

    public static bool TryParse(string value, out BarSpec? barSpec)
    {
        barSpec = null;

        if (string.IsNullOrWhiteSpace(value))
            return false;

        if (value.Length != 7)
            return false;

        if (!Enum.TryParse(value[0..2], out BarKind barKind))
            return false;

        if (!int.TryParse(value[3..], out int barSize))
            return false;

        barSpec = Create(barKind, barSize);

        return true;
    }

    public static BarSpec Create(BarKind barKind, int barSize)
    {
        barKind.Must().BeEnumValue();
        barSize.Must().Be(v => v.IsBarSize(barKind));

        return new BarSpec(barKind, barSize);
    }
}