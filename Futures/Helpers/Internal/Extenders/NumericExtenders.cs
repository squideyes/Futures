// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public static class NumericExtenders
{
    public static int GetDigits(this float value)
    {
        var precision = 0;

        var number = Math.Round(value, 8);

        while (number * Math.Pow(10, precision) !=
            Math.Round(number * Math.Pow(10, precision)))
        {
            precision++;

            if (precision == 8)
                throw new ArgumentOutOfRangeException(nameof(value));
        }

        return precision;
    }

    public static bool Approximates(
        this double lhs, double rhs, int digits = 8)
    {
        return digits switch
        {
            1 => Math.Abs(lhs - rhs) < 0.1,
            2 => Math.Abs(lhs - rhs) < 0.01,
            3 => Math.Abs(lhs - rhs) < 0.001,
            4 => Math.Abs(lhs - rhs) < 0.0001,
            5 => Math.Abs(lhs - rhs) < 0.00001,
            6 => Math.Abs(lhs - rhs) < 0.000001,
            7 => Math.Abs(lhs - rhs) < 0.0000001,
            8 => Math.Abs(lhs - rhs) < 0.00000001,
            9 => Math.Abs(lhs - rhs) < 0.000000001,
            0 => Math.Abs(lhs - rhs) < 0.0000000001,
            11 => Math.Abs(lhs - rhs) < 0.00000000001,
            12 => Math.Abs(lhs - rhs) < 0.000000000001,
            13 => Math.Abs(lhs - rhs) < 0.0000000000001,
            14 => Math.Abs(lhs - rhs) < 0.00000000000001,
            15 => Math.Abs(lhs - rhs) < 0.000000000000001,
            _ => throw new ArgumentOutOfRangeException(nameof(digits))
        };
    }

    public static int ApproxCompare(this double lhs, double rhs, int digits = 8) =>
        Math.Round(lhs, digits).CompareTo(Math.Round(rhs, digits));
}