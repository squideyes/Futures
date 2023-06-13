// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Futures.Models;

namespace SquidEyes.Testing;

public class TestData : IDisposable
{
    private readonly Dictionary<string, TickSet> tickSets = new();
    //private readonly Dictionary<Symbol, CandleSet> candleSets = new();

    public TestData()
    {
        foreach (var symbol in KnownSymbols.GetAll(Source.KibotHistory))
        {
            var asset = KnownAssets.Get(symbol);

            using var stream = new MemoryStream(GetBytes(symbol));

            //var tickSet = TickSet.LoadFrom(stream);

            //tickSets.Add(symbol, tickSet);

            //var candleSet = CandleSet.Create(
            //    tickSet.Contract, tickSet.TradeDate);

            //var feed = TdRenkoFeed.Create(asset, BrickTicks.From(8));

            //feed.OnCandle += (s, e) => candleSet.Add(e.Candle);

            //for (var i = 0; i < tickSet.Count; i++)
            //    feed.HandleTick(tickSet[i], i == tickSet.Count - 1);

            //candleSets.Add(symbol, candleSet);
        }
    }

    //public TickSet GetTickSet(Symbol symbol) => 
    //    tickSets[symbol];

    //public CandleSet GetCandleSet(Symbol symbol) => 
    //    candleSets[symbol];

    public void Dispose() => GC.SuppressFinalize(this);

    private static byte[] GetBytes(string symbol)
    {
        return symbol switch
        {
            "BP" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "CL" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "ES" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "EU" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "GC" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "JY" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "NQ" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "ZB" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "ZF" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            "ZN" => Properties.TestData.KBH_BP_20191216_TICK_EST,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }
}