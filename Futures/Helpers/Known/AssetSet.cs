// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;
using static SquidEyes.Futures.Models.Exchange;

namespace SquidEyes.Futures.Helpers;

public static class AssetSet
{
    private static readonly SortedDictionary<string, Asset> assets = new();

    static AssetSet()
    {
        static void Add(string symbol, Exchange exchange,
            string months, float oneTick, double tickInUsd, string description)
        {
            assets.Add(symbol, new Asset(symbol,
                exchange, months, oneTick, tickInUsd, description));
        }

        Add("BP", CME, "HMUZ", 0.0001f, 6.25, "BRITISH POUND");
        Add("CL", NYMEX, "FGHJKMNQUVXZ", 0.01f, 10.0, "CRUDE-OIL");
        Add("E7", CME, "HMUZ", 0.0001f, 6.25, "E-MINI EURO FX");
        Add("ES", CME, "HMUZ", 0.25f, 12.5, "E-MINI S&P 500");
        Add("EU", CME, "HMUZ", 0.00005f, 6.25, "EURO FX");
        Add("GC", COMEX, "GJMQVZ", 0.1f, 10.0, "GOLD");
        Add("J7", CME, "HMUZ", 0.000001f, 6.25, "E-MINI JAPANESE YEN");
        Add("JY", CME, "HMUZ", 0.0000005f, 6.25, "JAPANESE YEN");
        Add("MES", CME, "HMUZ", 0.25f, 1.25, "MICRO E-MINI S&P 500");
        Add("MNQ", CME, "HMUZ", 0.25f, 0.5, "MICRO E-MINI NASDAQ-100");
        Add("NQ", CME, "HMUZ", 0.25f, 5.0, "E-MINI NASDAQ-100");
        Add("QM", NYMEX, "FGHJKMNQUVXZ", 0.01f, 1.0, "E-MINI CRUDE-OIL");
        Add("QO", COMEX, "FGJMQVZ", 0.25f, 12.5000, "E-MINI GOLD");
        Add("ZB", CBOT, "HMUZ", 0.03125f, 31.25, "30 YR US TREASURY BOND");
        Add("ZN", CBOT, "HMUZ", 0.015625f, 15.625, "10 YR US TREASURY NOTE");
        Add("ZF", CBOT, "HMUZ", 0.0078125f, 7.8125, "5 YR US TREASURY NOTE");
    }

    public static int Count => assets.Count;

    public static Asset From(string symbol) => assets[symbol];

    public static bool Contains(string symbol) => assets.ContainsKey(symbol);

    public static Asset[] GetAll() => assets.Values.ToArray();

    public static Asset[] GetFiltered(Exchange? exchange = null)
    {
        return assets.Values.Where(
            a => !exchange.HasValue || a.Exchange == exchange).ToArray();
    }
}