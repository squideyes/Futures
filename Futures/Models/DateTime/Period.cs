// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public readonly struct Period<T>
    where T : struct, IComparable<T>
{
    internal Period(T from, T until)
    {
        if (!from.IsPeriodType())
            throw new ArgumentOutOfRangeException(nameof(until));

        if (until.CompareTo(from) <= 0)
            throw new ArgumentOutOfRangeException(nameof(until));

        From = from;
        Until = until;
    }

    public T From { get; }
    public T Until { get; }

    public (TickOn MinTickOn, TickOn MaxTickOn) GetMinAndMaxTickOn(TradeDate tradeDate)
    {
        var dateTime = tradeDate.AsDateTime();

        TickOn GetTickOn(T value)
        {
            return value switch
            {
                TimeOnly v => TickOn.From(dateTime.Add(new TimeSpan(
                    0, v.Hour, v.Minute, v.Second, v.Millisecond))),
                TimeSpan v => TickOn.From(dateTime.Add(v)),
                _ => throw new ArgumentOutOfRangeException(nameof(value))
            };
        }

        return (GetTickOn(From), GetTickOn(Until));
    }
}