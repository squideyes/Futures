// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Helpers;

public static class ContractExtenders
{
    private static readonly string MONTH_CODES = "FGHJKMNQUVXZ";

    public static string ToLetter(this Month month) =>
        MONTH_CODES[(int)month - 1].ToString();

    public static Month ToMonth(this int value) => (Month)value;

    public static SortedSet<Month> ToMonths(this string value) =>
        new(value.ToCharArray().Select(v => v.ToMonth()));

    public static Month ToMonth(this char value)
    {
        var index = MONTH_CODES.IndexOf(value);

        if (index == -1)
            throw new ArgumentOutOfRangeException(nameof(value));

        return (Month)(index + 1);
    }

    internal static int GetContractMonths(this Asset asset, Month month)
    {
        var months = asset.Months!.ToList();

        var index = months.IndexOf(month);

        if (index > 0)
            return month - months[index - 1];
        else
            return (int)months[0] + (MONTH_CODES.Length - (int)months.Last());
    }
}