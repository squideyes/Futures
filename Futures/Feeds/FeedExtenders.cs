// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Feeds;

public static class FeedExtenders
{
    public static bool IsTickSkip(this int value) =>
        value.IsBetween(0, 999);
}