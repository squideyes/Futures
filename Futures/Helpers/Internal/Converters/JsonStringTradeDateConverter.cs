// ************************************************************************
// Copyright (C) 2022 SquidEyes, LLC - All Rights Reserved
// Proprietary and confidential
// Unauthorized copying of this file, via any medium is strictly prohibited
// ************************************************************************

using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

public class JsonStringTradeDateConverter : JsonConverter<TradeDate>
{
    public override TradeDate Read(ref Utf8JsonReader reader,
        Type _, JsonSerializerOptions options)
    {
        return TradeDate.Parse(reader.GetString()!);
    }

    public override void Write(Utf8JsonWriter writer,
        TradeDate value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}