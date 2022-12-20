// ************************************************************************
// Copyright (C) 2022 SquidEyes, LLC - All Rights Reserved
// Proprietary and confidential
// Unauthorized copying of this file, via any medium is strictly prohibited
// ************************************************************************

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
            Kind = kind.Must().BeEnumValue(),
            Name = name.MayNot().BeNullOrWhitespace(),
            From = from,
            Until = until.Add(TimeSpan.FromMilliseconds(999)),
        };
    }

    internal static Embargo Create(
        string name, TimeOnly anchor, int preNewsMinutes, int postNewsMinutes)
    {
        return new Embargo()
        {
            Kind = NewsEvent,
            Name = name,
            From = anchor.Add(-TimeSpan.FromMinutes(preNewsMinutes)),
            Until = anchor.Add(new TimeSpan(0, 0, postNewsMinutes, 59, 999))
        };
    }
}