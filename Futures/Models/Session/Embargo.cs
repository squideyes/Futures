// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using static SquidEyes.Futures.EmbargoKind;

namespace SquidEyes.Futures;

public class Embargo
{
    public required EmbargoKind Kind { get; init; }
    public required string Name { get; init; }
    public required TimeOnly From { get; init; }
    public required TimeOnly Until { get; init; }

    internal bool IsEmbargoed(TickOn tickOn) =>
        TimeOnly.FromDateTime(tickOn.AsDateTime()).AsFunc(t => t >= From && t <= Until);

    public override string ToString()
    {
        var prefix = Kind switch
        {
            NewsEvent => "NEWS-EVENT",
            AdHoc => "AD-HOC EMBARGO",
            Strategy => "EMBARGO",
            _ => throw new ArgumentOutOfRangeException(nameof(Kind))
        };

        return $"{prefix}: {Name}; from {From} to {Until}";
    }

    internal static Embargo Create(
        EmbargoKind kind, string name, TimeOnly from, TimeOnly until)
    {
        return new Embargo()
        {
            Kind = kind,
            Name = name,
            From = from,
            Until = until
        };
    }

    internal static Embargo Create(string name, 
        TimeOnly anchor, int preNewsMinutes, int postNewsMinutes)
    {
        return new Embargo()
        {
            Kind = NewsEvent,
            Name = name,
            From = anchor.Add(-TimeSpan.FromMinutes(preNewsMinutes)),
            Until = anchor.Add(
                new TimeSpan(0, 0, postNewsMinutes - 1, 59, 999))
        };
    }
}