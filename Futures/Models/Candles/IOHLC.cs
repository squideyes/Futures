// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public interface IOHLC
{
    TickOn CloseOn { get; }
    float Open { get; }
    float High { get; }
    float Low { get; }
    float Close { get; }
}