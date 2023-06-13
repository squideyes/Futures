// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures.Helpers;

public class JsonStringTradeDateConverter : JsonConverter<TradeDate>
{
    public override TradeDate Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return KnownTradeDates.From(
            DateOnly.Parse(reader.GetString()!));
    }

    public override void Write(Utf8JsonWriter writer, 
        TradeDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(
            value.AsDateOnly().ToString("yyyy-MM-dd"));
    }
}