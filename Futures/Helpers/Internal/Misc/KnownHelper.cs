// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections.Immutable;
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

        var json = GetJson();

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

    public static ImmutableSortedDictionary<DateOnly, TradeDate> GetTradeDates()
    {
        var holidays = HolidayHelper.GetHolidays();

        var dates = new List<DateOnly>();

        for (var date = TradeDate.MinValue;
            date <= TradeDate.MaxValue; date = date.AddDays(1))
        {
            if (date.IsWeekDay() && !holidays.Contains(date))
                dates.Add(date);
        }

        return dates.ToImmutableSortedDictionary(d => d, d => new TradeDate(d));
    }

    public static Dictionary<Asset, List<Contract>> GetContracts(List<Asset> assets)
    {
        static List<Contract> GetContracts(Asset asset)
        {
            var contracts = new List<Contract>();

            for (int year = Contract.MinYear; year <= Contract.MaxYear; year++)
            {
                foreach (var month in asset.Months!)
                    contracts.Add(new Contract(asset, month, year));
            }

            return contracts;
        }

        var contracts = new Dictionary<Asset, List<Contract>>();

        foreach (var asset in assets)
            contracts.Add(asset, GetContracts(asset));

        return contracts;
    }


    private static string GetJson()
    {
        return """ 
            {
              "Markets": {
                "CME FX Futures ETH": {
                  "From": "-0.06:00:00.000",
                  "Until": "0.16:59:59.999"
                },
                "CME US Index Futures ETH": {
                  "From": "-0.06:00:00.000",
                  "Until": "0.16:59:59.999"
                },
                "CBOT Interest Rate ETH": {
                  "From": "-0.06:00:00.000",
                  "Until": "0.16:59:59.999"
                },
                "Nymex Metals Energy ETH": {
                  "From": "-0.06:00:00.000",
                  "Until": "0.16:59:59.999"
                }
              },
              "Assets": {
                "CL": {
                  "Market": "Nymex Metals Energy ETH",
                  "Session": {
                    "From": "09:00",
                    "Until": "14:30"
                  },
                  "Description": "Crude Oil Contract",
                  "TickInUsd": 10.0,
                  "OneTick": 0.01,
                  "Months": "FGHJKMNQUVXZ"
                },
                "ES": {
                  "Market": "CME US Index Futures ETH",
                  "Session": {
                    "From": "09:30",
                    "Until": "16:00"
                  },
                  "Description": "E-Mini S&P 500 Contract",
                  "TickInUsd": 12.5,
                  "OneTick": 0.25,
                  "Months": "HMUZ"
                },
                "EU": {
                  "Market": "CME FX Futures ETH",
                  "Session": {
                    "From": "08:00",
                    "Until": "12:00"
                  },
                  "Description": "EURO FX Futures",
                  "TickInUsd": 6.25,
                  "OneTick": 0.00005,
                  "Months": "HMUZ"
                },
                "GC": {
                  "Market": "Nymex Metals Energy ETH",
                  "Session": {
                    "From": "09:30",
                    "Until": "13:00"
                  },
                  "Description": "Gold Contract",
                  "TickInUsd": 10.0,
                  "OneTick": 0.1,
                  "Months": "GJMQZ"
                },
                "JY": {
                  "Market": "CME FX Futures ETH",
                  "Session": {
                    "From": "07:00",
                    "Until": "10:00"
                  },
                  "Description": "Japanese Yen FX Futures",
                  "TickInUsd": 6.25,
                  "OneTick": 0.0000005,
                  "Months": "HMUZ"
                },
                "NQ": {
                  "Market": "CME US Index Futures ETH",
                  "Session": {
                    "From": "09:30",
                    "Until": "16:00"
                  },
                  "Description": "E-Mini NASDAQ 100 Contract",
                  "TickInUsd": 5.0,
                  "OneTick": 0.25,
                  "Months": "HMUZ"
                },
                "TY": {
                  "Market": "CBOT Interest Rate ETH",
                  "Session": {
                    "From": "07:00",
                    "Until": "11:00"
                  },
                  "Description": "10 Year US Treasury Note Contract",
                  "TickInUsd": 15.625,
                  "OneTick": 0.015625,
                  "Months": "HMUZ"
                },
                "US": {
                  "Market": "CBOT Interest Rate ETH",
                  "Session": {
                    "From": "07:00",
                    "Until": "11:00"
                  },
                  "Description": "30 Year US Treasure Bond Contract",
                  "TickInUsd": 31.25,
                  "OneTick": 0.03125,
                  "Months": "HMUZ"
                }
              }
            }            
            """;
    }
}