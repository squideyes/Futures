using SquidEyes.Futures.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures.Helpers;

public class JsonStringContractConverter : JsonConverter<Contract>
{
    public override Contract Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return Contract.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer, 
        Contract value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}
