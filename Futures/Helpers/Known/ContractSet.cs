// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Helpers;

using SquidEyes.Futures.Models;

using CBATD = Dictionary<(Asset, TradeDate), Contract>;

public static class ContractSet
{
    private static readonly Dictionary<Asset, List<Contract>> contracts = new();

    private static readonly CBATD cbatd;

    static ContractSet()
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

        var assets = AssetSet.GetAll();

        foreach (var asset in assets)
            contracts.Add(asset, GetContracts(asset));

        cbatd = GetContractsByAssetTradeDate(assets);
    }

    public static Contract GetContract(Asset asset, TradeDate tradeDate) =>
        cbatd[(asset, tradeDate)];

    private static CBATD GetContractsByAssetTradeDate(Asset[] assets)
    {
        var cbatd = new CBATD();

        foreach (var asset in assets)
        {
            foreach (var contract in contracts[asset])
            {
                foreach (var tradeDate in contract.TradeDates)
                    cbatd.Add((asset, tradeDate), contract);
            }
        }

        return cbatd;
    }
}