// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Helpers;

public static class DateTimeExtenders
{
    public static DateOnly ToPotentialTradeDateValue(this DateTime value)
    {
        if (value.Hour >= 18)
            return DateOnly.FromDateTime(value.Date.AddDays(1));
        else
            return DateOnly.FromDateTime(value.Date);
    }

    public static bool IsTickOn(this DateTime value)
    {
        var date = value.ToPotentialTradeDateValue();

        if (!KnownTradeDates.TryGetTradeDate(date, out var tradeDate))
            return false;

        return tradeDate.IsTickOn(value);
    }

    public static string ToText(this DateTime value, DateTimeFormat format)
    {
        return format switch
        {
            DateTimeFormat.Default => value.ToString("MM/dd/yyyy HH:mm:ss.fff"),
            DateTimeFormat.Sortable => value.ToString("yyyyMMddHHmmssfff"),
            DateTimeFormat.DateOnly => value.ToString("MM/dd/yyyy"),
            DateTimeFormat.TimeOnly => value.ToString("HH:mm:ss.fff"),
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}