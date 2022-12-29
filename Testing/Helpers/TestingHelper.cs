// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using FluentAssertions;
using SquidEyes.Futures;

namespace SquidEyes.Testing;

public static class TestingHelper
{
    public static void AssertSourceMatchesTarget(
        TickSet source, TickSet target)
    {
        target.Contract.Should().Be(source.Contract);
        target.Count.Should().Be(source.Count);
        target.FileName.Should().Be(source.FileName);
        target.MaxTickOn.Should().Be(source.MaxTickOn);
        target.MinTickOn.Should().Be(source.MinTickOn);
        target.Source.Should().Be(source.Source);
        target.TradeDate.Should().Be(source.TradeDate);
        target.ToString().Should().Be(source.ToString());

        for (var i = 0; i < target.Count; i++)
            target[i].Should().Be(source[i]);
    }
}