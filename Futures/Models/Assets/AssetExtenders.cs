// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

internal static class AssetExtenders
{
    private static readonly string MONTH_CODES = "FGHJKMNQUVXZ";

    public static string ToMonthCode(this Month month) =>
        MONTH_CODES[(int)month - 1].ToString();

    public static Month ToMonth(this int value) => (Month)value;

    public static Month ToMonth(this char value) =>
        (Month)(MONTH_CODES.IndexOf(value) + 1);

    public static int GetContractMonths(this Asset asset, Month month)
    {
        var months = asset.Months!.ToList();

        var index = months.IndexOf(month);

        if (index > 0)
            return month - months[index - 1];
        else
            return (int)months[0] + (MONTH_CODES.Length - (int)months.Last());
    }

    public static Month ToMonth(this string value) => ToMonth(value[0]);
}