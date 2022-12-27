// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections.Immutable;

namespace SquidEyes.Futures;

using CBATD = Dictionary<(Asset, TradeDate), Contract>;

public static class Known
{
    private static readonly TimeSpan minOffset = TimeSpan.FromHours(-6);
    private static readonly TimeSpan maxOffset = new(0, 16, 59, 59, 999);

    private static readonly CBATD cbatd;

    static Known()
    {
        TradeDates = GetTradeDates();
        TickOnOffsets = new Stretch<TimeSpan>(minOffset, maxOffset);
        SymbolAs = new SymbolAs();
        Assets = GetAssets();
        Contracts = GetContracts(Assets.Values.ToList());

        cbatd = GetContractsByAssetTradeDate();
    }

    public static ImmutableSortedDictionary<DateOnly, TradeDate> TradeDates { get; }
    public static Stretch<TimeSpan> TickOnOffsets { get; }
    public static SymbolAs SymbolAs { get; }
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
        var assets = new List<Asset>();

        static TimeOnly From(int hour, int minute) => new(hour, minute, 0);

        static TimeOnly Until(int hour, int minute) => new(hour, minute, 59, 999);

        void Add(Symbol symbol, Exchange exchange, string months, float oneTick,
            double tickInUsd, string description, TimeOnly from, TimeOnly until)
        {
            assets.Add(new Asset(symbol, exchange, months,
                oneTick, tickInUsd, description, from, until));
        }

        Add(Symbol.MES, Exchange.CME, "HMUZ", 0.25f, 1.25,
            "MICRO E-MINI S&P 500", From(9, 30), Until(15, 59));
        Add(Symbol.ES, Exchange.CME, "HMUZ", 0.25f, 12.5,
            "E-MINI S&P 500", From(9, 30), Until(15, 59));
        Add(Symbol.MNQ, Exchange.CME, "HMUZ", 0.25f, 0.5,
            "MICRO E-MINI NASDAQ-100", From(9, 30), Until(15, 59));
        Add(Symbol.NQ, Exchange.CME, "HMUZ", 0.25f, 5.0, 
            "E-MINI NASDAQ-100", From(9, 30), Until(15, 59));
        Add(Symbol.CL, Exchange.NYMEX, "FGHJKMNQUVXZ", 0.01f,
            10.0, "CRUDE-OIL", From(9, 0), Until(14, 29));
        Add(Symbol.QM, Exchange.NYMEX, "FGHJKMNQUVXZ", 0.01f,
            1.0, "E-MINI CRUDE-OIL", From(9, 0), Until(14, 29));
        Add(Symbol.ZB, Exchange.CBOT, "HMUZ", 0.03125f, 31.25, 
            "30 YR US TREASURY BOND", From(7, 0), Until(10, 59));
        Add(Symbol.ZN, Exchange.CBOT, "HMUZ", 0.015625f, 15.625,
            "10 YR US TREASURY NOTE", From(7, 0), Until(10, 59));
        Add(Symbol.GC, Exchange.COMEX, "GJMQVZ", 0.1f, 
            10.0, "GOLD", From(9, 30), Until(12, 59));
        Add(Symbol.QO, Exchange.COMEX, "FGJMQVZ", 0.25f,
            12.5000, "E-MINI GOLD", From(9, 30), Until(12, 59));
        Add(Symbol.EU, Exchange.CME, "HMUZ", 0.00005f, 6.25,
            "EURO FX", From(8, 0), Until(11, 59));
        Add(Symbol.E7, Exchange.CME, "HMUZ", 0.0001f, 6.25,
            "E-MINI EURO FX", From(8, 0), Until(11, 59));
        Add(Symbol.JY, Exchange.CME, "HMUZ", 0.0000005f, 6.25,
            "JAPANESE YEN", From(7, 0), Until(9, 59));
        Add(Symbol.J7, Exchange.CME, "HMUZ", 0.000001f, 6.25,
            "E-MINI JAPANESE YEN", From(7, 0), Until(9, 59));
        Add(Symbol.BP, Exchange.CME, "HMUZ", 0.0001f, 6.25,
            "BRITISH POUND", From(4, 0), Until(11, 59));
        Add(Symbol.ZF, Exchange.CBOT, "HMUZ", 0.0078125f, 7.8125, 
            "5 YR US TREASURY NOTE", From(7, 0), Until(10, 59));

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