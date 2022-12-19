// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class Market
{
    public required string Name { get; init; }
    public required Period<TimeSpan> Period { get; init;  }
}