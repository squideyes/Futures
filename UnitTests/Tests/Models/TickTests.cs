// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.UnitTests;

public class TickTests
{
    [Fact]
    public void Tick_WithGoodFromArgs_Should_Contruct()
    {
        var asset = Known.Assets[Symbol.NQ];

        var tickOn = TickOn.From(
            new DateTime(2022, 7, 17, 18, 0, 0, 344));

        _ = Tick.From(asset, 0, tickOn, 12049.5f);
    }
}