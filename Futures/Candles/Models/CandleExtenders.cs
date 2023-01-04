// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public static class CandleExtenders
{
    public static bool IsInterval(this int value) =>
        value.IsBetween(5, 3600) && value % 5 == 0;
}