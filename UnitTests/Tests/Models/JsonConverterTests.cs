// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Futures.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.UnitTests;

public class JsonConverterTests
{
    [Fact]
    public void Contract_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<Contract, JsonStringContractConverter>(
            KnownAssets.GetAssets().First().Contracts.First());
    }

    [Fact]
    public void Asset_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<Asset, JsonStringAssetConverter>(
            KnownAssets.GetAssets().First());
    }

    [Fact]
    public void TradeDate_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<TradeDate, JsonStringTradeDateConverter>(
            KnownTradeDates.GetAll().First());
    }

    private static void Roundtrip<T, C>(T source)
        where C : JsonConverter, new()
    {
        var options = new JsonSerializerOptions();

        options.Converters.Add(new C());

        var json = JsonSerializer.Serialize(source, options);

        var target = JsonSerializer.Deserialize<T>(json, options);

        source.Should().Be(target!);
    }
}