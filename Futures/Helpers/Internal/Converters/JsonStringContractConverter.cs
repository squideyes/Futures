// ************************************************************************
// Copyright (C) 2022 SquidEyes, LLC - All Rights Reserved
// Proprietary and confidential
// Unauthorized copying of this file, via any medium is strictly prohibited
// ************************************************************************

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

public class JsonStringContractConverter : JsonConverter<Contract>
{
    public override Contract Read(ref Utf8JsonReader reader,
        Type _, JsonSerializerOptions options)
    {
        return Contract.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer,
        Contract value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}