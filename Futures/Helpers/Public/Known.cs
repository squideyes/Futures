// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections.Immutable;
using System.Text;

namespace SquidEyes.Futures;

using CBATD = Dictionary<(Asset, TradeDate), Contract>;

public static class Known
{
    private static readonly CBATD cbatd;

    static Known()
    {
        TradeDates = KnownHelper.GetTradeDates();
        Assets = KnownHelper.GetAssets();
        Contracts = KnownHelper.GetContracts(Assets.Values.ToList());

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

    private static string GetContractsSpecs()
    {
        var sb = new StringBuilder();

        var yesterday = DateTime.Today;

        foreach (var asset in Assets.Values)
        {
            foreach (var contract in Contracts[asset])
            {
                var until = contract.TradeDates.Last().AsDateTime();

                if (until >= yesterday)
                    continue;

                var from = contract.TradeDates.First()
                    .AsDateTime().Add(asset.Market!.Period.From);

                until = until.Add(asset.Market.Period.Until);

                sb.Append(asset.Symbol);
                sb.AppendDelimited(((int)contract.Month).ToString("00"));
                sb.AppendDelimited(contract.Year);
                sb.AppendDelimited(contract);
                sb.AppendDelimited(from.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                sb.AppendDelimited(until.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }
}