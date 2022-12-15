namespace SquidEyes.Futures;

internal static class AssetExtenders
{
    private static readonly string MONTH_CODES = "FGHJKMNQUVXZ";

    public static string ToMonthCode(this Month month) =>
        MONTH_CODES[(int)month - 1].ToString();

    public static Month ToMonth(this int value) => (Month)value;

    public static Month ToMonth(this char value) =>
        (Month)(MONTH_CODES.IndexOf(value) + 1);

    public static Month ToMonth(this string value) => ToMonth(value[0]);
}