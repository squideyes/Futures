//// ********************************************************
//// The use of this source code is licensed under the terms
//// of the MIT License (https://opensource.org/licenses/MIT)
//// ********************************************************

//using SquidEyes.Futures;
//using FluentAssertions;

//namespace SquidEyes.UnitTests;

//public class TickOnTests
//{
//    [Fact]
//    public void TickOn_WithGoodFromArgs_Should_Construct()
//    {
//        var dateTime = new DateTime(2022, 5, 2, 3, 4, 5, 6);

//        var tickOn = TickOn.From(dateTime);

//        var tradeDate = tickOn.AsTradeDate();

//        tickOn.Year.Should().Be(dateTime.Year);
//        tickOn.Month.Should().Be(dateTime.Month);
//        tickOn.Day.Should().Be(dateTime.Day);
//        tickOn.Hour.Should().Be(dateTime.Hour);
//        tickOn.Minute.Should().Be(dateTime.Minute);
//        tickOn.Second.Should().Be(dateTime.Second);
//        tickOn.Millisecond.Should().Be(dateTime.Millisecond);
//        tickOn.DayOfWeek.Should().Be(dateTime.DayOfWeek);

//        tickOn.AsDateTime().Should().Be(dateTime);
//        tickOn.AsTradeDate().Should().Be(tradeDate);

//        tickOn.ToString().Should().Be("05/02/2022 03:04:05.006");

//        tickOn.GetHashCode().Should().Be(dateTime.GetHashCode());
//    }

//    [Fact]
//    public void TickOn_Operators_Should_Work()
//    {
//        var a = TickOn.From(new DateTime(2022, 5, 2, 10, 0, 0, 0));
//        var b = TickOn.From(new DateTime(2022, 5, 2, 10, 0, 0, 0));
//        var c = TickOn.From(new DateTime(2022, 5, 2, 10, 0, 0, 1));
//        var d = TickOn.From(new DateTime(2022, 5, 2, 10, 0, 0, 2));

//        a.Equals((object)b).Should().BeTrue();
//        a.Should().Be(b);
//        b.Should().NotBe(c);

//        (a == b).Should().BeTrue();
//        (b != c).Should().BeTrue();

//        (b < c).Should().BeTrue();
//        (a <= b).Should().BeTrue();
//        (d > c).Should().BeTrue();
//        (a >= b).Should().BeTrue();

//        b.CompareTo(c).Should().Be(-1);
//        a.CompareTo(b).Should().Be(0);
//        d.CompareTo(c).Should().Be(1);
//        a.CompareTo(d).Should().Be(-1);
//    }
//}