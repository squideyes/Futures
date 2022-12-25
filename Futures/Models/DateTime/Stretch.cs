// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public readonly struct Stretch
{
    internal Stretch(TimeOnly from, TimeOnly until)
    {
        From = from;
        Until = until;
    }

    public TimeOnly From { get; }
    public TimeOnly Until { get; }

    public (TickOn MinTickOn, TickOn MaxTickOn) GetMinAndMaxTickOn(
        TradeDate tradeDate)
    {
        var dateTime = tradeDate.AsDateTime();

        TickOn GetTickOn(TimeOnly value)
        {
            return TickOn.From(dateTime.Add(new TimeSpan(0, value.Hour,
                value.Minute, value.Second, value.Millisecond)));
        }

        return (GetTickOn(From), GetTickOn(Until));
    }
}