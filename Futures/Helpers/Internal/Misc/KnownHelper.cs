using System.Text.Json;
using System.Text.Json.Serialization;

namespace SquidEyes.Futures;

internal static class KnownHelper
{
    private class AssetInfo
    {
        public double TickInUsd { get; set; }
        public float OneTick { get; set; }
        public string? Months { get; set; }
        public string? Description { get; set; }
        public string? Market { get; set; }
        public PeriodInfo? Session { get; set; }
    }

    private class MarketInfo
    {
        public TimeSpan From { get; set; }
        public TimeSpan Until { get; set; }
    }

    private class PeriodInfo
    {
        public TimeOnly From { get; set; }
        public TimeOnly Until { get; set; }
    }

    private class Data
    {
        [JsonPropertyName("Markets")]
        public Dictionary<string, MarketInfo>? MarketInfos { get; set; }

        [JsonPropertyName("Assets")]
        public Dictionary<Symbol, AssetInfo>? AssetInfos { get; set; }
    }

    public static Dictionary<Symbol, Asset> GetAssets()
    {
        var options = new JsonSerializerOptions();

        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new JsonStringTimeOnlyConverter());

        var json = Properties.Resources.Known;

        var datas = JsonSerializer.Deserialize<Data>(json, options);

        var markets = new Dictionary<string, Market>();

        foreach (var m in datas!.MarketInfos!)
        {
            markets.Add(m.Key, new Market
            {
                Name = m.Key,
                Period = new Period<TimeSpan>(m.Value.From, m.Value.Until) 
            });
        }

        var assets = new Dictionary<Symbol, Asset>();

        foreach (var symbol in datas!.AssetInfos!.Keys)
        {
            var data = datas!.AssetInfos[symbol];

            var digits = data.OneTick.GetDigits();

            var ticksPerPoint = (int)(1.0f / data.OneTick);

            Asset GetAsset(Symbol symbol)
            {
                var tickInUsd = data.TickInUsd;

                return new Asset(digits)
                {
                    Symbol = symbol,
                    Description = data.Description,
                    OneTick = data.OneTick,
                    Digits = digits,
                    TickInUsd = Math.Round(tickInUsd, 2),
                    TicksPerPoint = ticksPerPoint,
                    Session = new Period<TimeOnly>(data.Session!.From, 
                        data.Session!.Until.Add(TimeSpan.FromMilliseconds(-1))),
                    Market = markets[data.Market!],
                    PointInUsd = Math.Round(
                        tickInUsd * ticksPerPoint, 2),
                    Months = new SortedSet<Month>(data.Months!
                        .ToCharArray().Select(v => v.ToMonth()))
                };
            }

            assets.Add(symbol, GetAsset(symbol));
        }

        return assets;
    }
}