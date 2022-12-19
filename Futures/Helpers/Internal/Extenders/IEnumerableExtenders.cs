// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public static class IEnumerableExtenders
{
    public static bool IsNotEmpty<T>(this IEnumerable<T> value) =>
        value is not null && value.Any() && !value.Any(v => v.IsDefault());
}