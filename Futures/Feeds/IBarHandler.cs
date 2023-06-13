// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures.Feeds;

public interface IBarHandler
{
    Task HandleBarAsync(BarArgs args);
}