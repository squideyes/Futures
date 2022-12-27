// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Runtime.InteropServices;

namespace SquidEyes.TickSetMaker;

internal static class MiscExtenders
{
    public static bool IsRootedPath(this string path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return false;

        if (path.IndexOfAny(Path.GetInvalidPathChars()) != -1)
            return false;

        return Path.IsPathRooted(path);
    }

    private static readonly TimeZoneInfo eastern =
        GetTimeZoneInfo("Eastern Standard Time", "America/New_York");

    public static DateTime ToEasternFromUtc(this DateTime value) =>
        value.ToZoneFromUtc(eastern);

    private static DateTime ToZoneFromUtc(this DateTime value, TimeZoneInfo tzi)
    {
        if (value.Kind != DateTimeKind.Utc)
            throw new ArgumentOutOfRangeException(nameof(value));

        return TimeZoneInfo.ConvertTimeFromUtc(value, tzi);
    }

    private static TimeZoneInfo GetTimeZoneInfo(string windowsId, string linuxId)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            return TimeZoneInfo.FindSystemTimeZoneById(windowsId);
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            return TimeZoneInfo.FindSystemTimeZoneById(linuxId);
        else
            throw new InvalidOperationException("Only works on Linux and Windows");
    }
}