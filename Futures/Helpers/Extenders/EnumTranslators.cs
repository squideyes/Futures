// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Helpers;

public static class EnumTranslators
{
    public static string ToCode(this Source source)
    {
        return source switch
        {
            Source.RithmicLive => "RTL",
            Source.RithmicPaper => "RTP",
            Source.KibotHistory => "KBH",
            _ => throw new ArgumentOutOfRangeException(nameof(source))
        };
    }

    public static Source ToSource(this string value)
    {
        return value switch
        {
            "RTL" => Source.RithmicLive,
            "RTP" => Source.RithmicPaper,
            "KBH" => Source.KibotHistory,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static string ToCode(this BarKind barKind)
    {
        return barKind switch
        {
            BarKind.Seconds => "SEC",
            BarKind.Minutes => "MIN",
            BarKind.Range => "RAN",
            BarKind.Renko => "REN",
            BarKind.TdRenko => "TDR",
            BarKind.TickCount => "TIC",
            _ => throw new ArgumentOutOfRangeException(nameof(barKind))
        };
    }

    public static BarKind ToBarKind(this string value)
    {
        return value switch
        {
            "SEC" => BarKind.Seconds,
            "MIN" => BarKind.Minutes,
            "RAN" => BarKind.Range,
            "REN" => BarKind.Renko,
            "TDR" => BarKind.TdRenko,
            "TIC" => BarKind.TickCount,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static bool IsBarSize(this int quantity, BarKind barKind)
    {
        return barKind switch
        {
            BarKind.Seconds => quantity.IsBetween(1, 3600),
            BarKind.Minutes => quantity.IsBetween(1, 60),
            BarKind.Renko => quantity.IsBetween(1, 100),
            BarKind.TdRenko => quantity.IsBetween(1, 100),
            BarKind.Range => quantity.IsBetween(1, 200),
            BarKind.TickCount => quantity.IsBetween(1, 200),
            _ => throw new ArgumentOutOfRangeException(nameof(quantity))
        };
    }
}