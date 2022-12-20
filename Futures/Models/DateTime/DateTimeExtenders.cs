namespace SquidEyes.Futures;

public static class DateTimeExtenders
{
    public static bool TryGetTradeDate(
        this DateTime value, out TradeDate tradeDate)
    {
        return Known.TradeDates.TryGetValue(
            value.ToTradeDateValue(), out tradeDate);
    }

    public static DateOnly ToTradeDateValue(this DateTime value)
    {
        if (value.Hour >= 18)
            return DateOnly.FromDateTime(value.Date.AddDays(1));
        else
            return DateOnly.FromDateTime(value.Date);
    }
}
