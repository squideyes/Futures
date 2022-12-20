// ************************************************************************
// Copyright (C) 2022 SquidEyes, LLC - All Rights Reserved
// Proprietary and confidential
// Unauthorized copying of this file, via any medium is strictly prohibited
// ************************************************************************

using SquidEyes.Futures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

public class JsonStringTickOnConverter : JsonConverter<TickOn>
{
    public override TickOn Read(ref Utf8JsonReader reader,
        Type _, JsonSerializerOptions options)
    {
        return TickOn.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer,
        TickOn value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}