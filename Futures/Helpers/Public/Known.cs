// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections.Immutable;

namespace SquidEyes.Futures;

using CBATD = Dictionary<(Asset, TradeDate), Contract>;

public static class Known
{
    private static readonly CBATD cbatd;

    static Known()
    {
        TradeDates = GetTradeDates();
        Assets = GetAssets();
        Contracts = GetContracts(Assets.Values.ToList());

        cbatd = GetContractsByAssetTradeDate();
    }

    public static ImmutableSortedDictionary<DateOnly, TradeDate> TradeDates { get; }
    public static IReadOnlyDictionary<Symbol, Asset> Assets { get; }
    public static IReadOnlyDictionary<Asset, List<Contract>> Contracts { get; }

    public static Contract GetContract(Asset asset, TradeDate tradeDate) =>
        cbatd[(asset, tradeDate)];

    private static CBATD GetContractsByAssetTradeDate()
    {
        var cbatd = new CBATD();

        foreach (var asset in Assets.Values)
        {
            foreach (var contract in Contracts[asset])
            {
                foreach (var tradeDate in contract.TradeDates)
                    cbatd.Add((asset, tradeDate), contract);
            }
        }

        return cbatd;
    }

    private static Dictionary<Symbol, Asset> GetAssets()
    {
        var cmeFxFutures = new Market("CME FX Futures ETH");
        var cmeUsIndex = new Market("CME US Index Futures ETH");
        var cbotInterest = new Market("CBOT Interest Rate ETH");
        var nymexEnergy = new Market("Nymex Energy Metals ETH");

        var assets = new List<Asset>();

        static TimeOnly From(int hour, int minute) => new(hour, minute, 0);

        static TimeOnly Until(int hour, int minute) => new(hour, minute, 59, 999);

        assets.Add(new Asset(Symbol.CL, 0.01f, 10.0, nymexEnergy,
            "Crude Oil Contract", "FGHJKMNQUVXZ", From(9, 0), Until(14, 29)));

        assets.Add(new Asset(Symbol.ES, 0.25f, 12.5, cmeUsIndex,
            "E-Mini S&P 500 Contract", "HMUZ", From(9, 30), Until(15, 59)));

        assets.Add(new Asset(Symbol.EU, 0.00005f, 6.25, cmeFxFutures,
            "EURO FX Futures", "HMUZ", From(8, 0), Until(11, 59)));

        assets.Add(new Asset(Symbol.GC, 0.1f, 10.0, nymexEnergy,
            "Gold Contract", "GJMQZ", From(9, 30), Until(12, 59)));

        assets.Add(new Asset(Symbol.JY, 0.0000005f, 6.25, cmeFxFutures,
            "Japanese Yen FX Futures", "HMUZ", From(7, 0), Until(9, 59)));

        assets.Add(new Asset(Symbol.NQ, 0.25f, 5.0, cmeUsIndex,
            "E-Mini NASDAQ 100 Contract", "HMUZ", From(9, 30), Until(15, 59)));

        assets.Add(new Asset(Symbol.TY, 0.015625f, 15.625, cbotInterest,
            "10 Year US Treasury Note Contract", "HMUZ", From(7, 0), Until(10, 59)));

        assets.Add(new Asset(Symbol.US, 0.03125f, 31.25, cbotInterest,
            "30 Year US Treasury Bond Contract", "HMUZ", From(7, 0), Until(10, 59)));

        return assets.ToDictionary(a => a.Symbol);
    }

    private static ImmutableSortedDictionary<DateOnly, TradeDate> GetTradeDates()
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

    private static Dictionary<Asset, List<Contract>> GetContracts(
        List<Asset> assets)
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
}