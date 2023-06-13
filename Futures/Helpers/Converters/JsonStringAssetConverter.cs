// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures.Helpers;

public class JsonStringAssetConverter : JsonConverter<Asset>
{
    public override Asset Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return KnownAssets.Get(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, 
        Asset value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}