// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.UnitTests;

public class JsonConverterTests
{
    [Fact]
    public void TickOn_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<TickOn, JsonStringTickOnConverter>(
            TickOn.From(new DateTime(2022, 5, 2, 9, 30, 1, 3)));
    }

    [Fact]
    public void Contract_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<Contract, JsonStringContractConverter>(
            Known.Contracts[Known.Assets[Symbol.NQ]].First());
    }

    [Fact]
    public void TimeOnly_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<TimeOnly, JsonStringTimeOnlyConverter>(
            new TimeOnly(1, 2, 3, 4));
    }

    [Fact]
    public void TradeDate_JsonConverter_Should_Roundtrip()
    {
        Roundtrip<TradeDate, JsonStringTradeDateConverter>(
            Known.TradeDates.First().Value);
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