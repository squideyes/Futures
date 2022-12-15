﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

internal class JsonStringTimeOnlyConverter : JsonConverter<TimeOnly>
{
    public override TimeOnly Read(ref Utf8JsonReader reader,
        Type _, JsonSerializerOptions options)
    {
        return TimeOnly.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer,
        TimeOnly value, JsonSerializerOptions options)
    {
        throw new InvalidOperationException();
    }
}