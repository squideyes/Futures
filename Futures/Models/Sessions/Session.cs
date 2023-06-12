using SquidEyes.Fundamentals;
using SquidEyes.Futures.Helpers;
using System.Collections;

namespace SquidEyes.Futures.Models;

public class Session : IEnumerable<TradeDate>
{
    private readonly List<TradeDate> tradeDates = new();

    public Session(TradeDate tradeDate, DataSpan dataSpan)
    {
        TradeDate = tradeDate.Must().Be(TradeDateSet.Contains);
        DataSpan = dataSpan.Must().BeEnumValue();

        tradeDates.Add(tradeDate);

        if (dataSpan == DataSpan.Week)
        {
            var date = tradeDate.AsDateOnly().AddDays(1);

            while (date.DayOfWeek <= DayOfWeek.Friday)
            {
                if (TradeDateSet.TryGetTradeDate(date, out tradeDate))
                    tradeDates.Add(tradeDate);

                date = date.AddDays(1);
            }
        }

        MinTickOn = tradeDates.First().MinTickOn;
        MaxTickOn = tradeDates.Last().MaxTickOn;
    }

    public TradeDate TradeDate { get; }
    public DataSpan DataSpan { get; }
    public DateTime MinTickOn { get; }
    public DateTime MaxTickOn { get; }

    public bool IsInSession(DateTime tickOn)
    {
        foreach (var tradeDate in tradeDates)
        {
            if (tradeDate.IsTickOn(tickOn))
                return true;
        }

        return false;
    }

    public IEnumerator<TradeDate> GetEnumerator() =>
        tradeDates.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
