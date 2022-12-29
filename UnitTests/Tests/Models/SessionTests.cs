// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using FluentAssertions;
using SquidEyes.Futures;

namespace SquidEyes.UnitTests;

public class SessionTests
{
    [Fact]
    public void Session_WithGoodFromArgs_Should_Construct()
    {
        var asset = Known.Assets[Symbol.ES];
        var tradeDate = TradeDate.From(2021, 12, 13);

        var session = Session.From(asset, tradeDate, 5, 10, 15);

        static TimeOnly From(int hour, int minute) => new(hour, minute);

        static TimeOnly Until(int hour, int minute) => 
            new(hour, minute, 59, 999);

        session.AddNewsEvent(From(10, 0), "News Event");

        session.AddEmbargo(From(12, 0), Until(12, 59), "Lunch", false);
        
        session.AddEmbargo(From(14, 0), Until(14, 29), "Ad-Hoc Break", true);

        session.TradeDate.Should().Be(tradeDate);

        session.MinDateTime.Should().Be(
            new DateTime(2021, 12, 13, 9, 30, 0));

        session.MaxDateTime.Should().Be(
            new DateTime(2021, 12, 13, 15, 59, 59, 999));

        session.MinTimeOnly.Should().Be(
            new TimeOnly(9, 30, 0));

        session.MaxTimeOnly.Should().Be(
            new TimeOnly(15, 59, 59, 999));

        session.MinTickOn.Should().Be(
            TickOn.From(session.MinDateTime));

        session.MaxTickOn.Should().Be(
            TickOn.From(session.MaxDateTime));

        session.Embargoes.Count.Should().Be(3);

        session.IsEmbargoed(TickOn.From(
            new DateTime(2021, 12, 13, 15, 44, 59, 999)),
            out Embargo e1).Should().BeFalse();

        e1.Should().BeNull();

        session.IsEmbargoed(TickOn.From(
            new DateTime(2021, 12, 13, 15, 45, 0)),
            out Embargo e2).Should().BeTrue();

        e2.Should().NotBeNull();
        e2.Kind.Should().Be(EmbargoKind.Strategy);
        e2.From.Should().Be(From(15, 45));
        e2.Until.Should().Be(Until(15, 59));

        session.IsEmbargoed(TickOn.From(
            new DateTime(2021, 12, 13, 15, 59, 59, 999)),
            out Embargo e3).Should().BeTrue();

        e3.Should().NotBeNull();
        e2.Kind.Should().Be(EmbargoKind.Strategy);
        e2.From.Should().Be(From(15, 45));
        e2.Until.Should().Be(Until(15, 59));

        void IsEmbargo(int index,
            EmbargoKind kind, TimeOnly from, TimeOnly until)
        {
            var embargo = session.Embargoes[index];

            embargo.Kind.Should().Be(kind);
            embargo.From.Should().Be(from);
            embargo.Until.Should().Be(until);
        }

        IsEmbargo(0, EmbargoKind.NewsEvent, 
            From(9, 55), new TimeOnly(10, 9, 59, 999));

        IsEmbargo(1, EmbargoKind.Strategy,
            From(12, 0), new TimeOnly(12, 59, 59, 999));

        IsEmbargo(2, EmbargoKind.AdHoc,
            From(14, 0), new TimeOnly(14, 29, 59, 999));
    }
}