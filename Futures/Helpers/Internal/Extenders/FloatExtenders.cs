// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

internal static class FloatExtenders
{
    public static int GetDigits(this float value)
    {
        int digits = 0;

        while (MathF.Round(value, digits) != value)
            digits++;

        return digits;
    }
}