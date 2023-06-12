// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using SquidEyes.Fundamentals;
using System.Linq.Expressions;
using System.Text.Json;

namespace SquidEyes.Futures.Helpers;

public static class JsonElementExtenders
{
    public static T GetEnumValue<M, T>(
        this JsonElement element, Expression<Func<M, T>> e)
        where T : struct, Enum
    {
        return GetElement(element, e).GetString()!.ToEnumValue<T>();
    }

    public static bool GetBoolean<M>(
        this JsonElement element, Expression<Func<M, bool>> e)
    {
        return GetElement(element, e).GetBoolean();
    }

    public static int GetInt32<M>(
        this JsonElement element, Expression<Func<M, int>> e)
    {
        return GetElement(element, e).GetInt32();
    }

    public static ClientId GetClientId<M>(
        this JsonElement element, Expression<Func<M, ClientId>> e)
    {
        return ClientId.From(GetElement(element, e).GetString()!);
    }

    public static Asset GetAsset<M>(
        this JsonElement element, Expression<Func<M, Asset>> e)
    {
        return AssetSet.From(GetElement(element, e).GetString()!);
    }

    public static MultiTag GetMultiTag<M>(
        this JsonElement element, Expression<Func<M, MultiTag>> e)
    {
        return MultiTag.From(GetElement(element, e).GetString()!);
    }

    public static string GetString<M>(
        this JsonElement element, Expression<Func<M, string>> e)
    {
        return GetElement(element, e).GetString()!;
    }

    private static JsonElement GetElement<M, T>(
        this JsonElement element, Expression<Func<M, T>> e)
    {
        return element.GetProperty(((MemberExpression)e.Body).Member.Name);
    }


    public static bool TryGetBoolean(this JsonElement element, out bool parsed)
    {
        var (p, wasParsed) = element.ValueKind switch
        {
            JsonValueKind.True => (true, true),
            JsonValueKind.False => (false, true),
            _ => (default, false)
        };

        parsed = p;

        return wasParsed;
    }

    public static bool TryGetString(this JsonElement element, out string parsed)
    {
        var (p, wasParsed) = element.ValueKind switch
        {
            JsonValueKind.String => (element.GetString(), true),
            JsonValueKind.Null => (null, true),
            _ => (default, false)
        };

        parsed = p!;

        return wasParsed;
    }
}