// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

public class JsonStringAssetConverter : JsonConverter<Asset>
{
    public override Asset Read(ref Utf8JsonReader reader,
        Type _, JsonSerializerOptions options)
    {
        return Known.Assets[
            Enum.Parse<Symbol>(reader.GetString()!)];
    }

    public override void Write(Utf8JsonWriter writer,
        Asset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}