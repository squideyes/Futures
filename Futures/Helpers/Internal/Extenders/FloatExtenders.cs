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
