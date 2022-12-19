// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using static System.DayOfWeek;

namespace SquidEyes.Futures;

internal static class DateOnlyExtenders
{
    public static bool IsWeekDay(this DateOnly date) =>
        date.DayOfWeek >= Monday && date.DayOfWeek <= Friday;

    public static bool IsWeekend(this DateOnly date) =>
        date.DayOfWeek == Saturday || date.DayOfWeek == Sunday;
}