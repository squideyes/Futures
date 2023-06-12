// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using System.Linq.Expressions;
using TD = SquidEyes.Testing.Properties.TestData;

namespace SquidEyes.Testing;

public class TestData : IDisposable
{
    //private readonly Dictionary<Symbol, TickSet> tickSets = new();
    //private readonly Dictionary<Symbol, CandleSet> candleSets = new();

    public TestData()
    {
        //var symbols = new Symbol[] 
        //{ 
        //    Symbol.GC, 
        //    Symbol.NQ, 
        //    Symbol.ZF 
        //};

        //foreach (var symbol in symbols)
        //{
        //    var asset = Known.Assets[symbol];

        //    using var stream = new MemoryStream(GetBytes(symbol));

        //    var tickSet = TickSet.From(stream);

        //    tickSets.Add(symbol, tickSet);

        //    var candleSet = CandleSet.Create(
        //        tickSet.Contract, tickSet.TradeDate);

        //    var feed = TdRenkoFeed.Create(asset, BrickTicks.From(8));

        //    feed.OnCandle += (s, e) => candleSet.Add(e.Candle);

        //    for (var i = 0; i < tickSet.Count; i++)
        //        feed.HandleTick(tickSet[i], i == tickSet.Count - 1);

        //    candleSets.Add(symbol, candleSet);
        //}
    }

    //public TickSet GetTickSet(Symbol symbol) => 
    //    tickSets[symbol];

    //public CandleSet GetCandleSet(Symbol symbol) => 
    //    candleSets[symbol];

    public void Dispose() => GC.SuppressFinalize(this);

    //private static byte[] GetBytes(Symbol symbol)
    //{
    //    return symbol switch
    //    {
    //        Symbol.GC => TD.KB_GC_20191216_TP_EST,
    //        Symbol.NQ => TD.KB_NQ_20191216_TP_EST,
    //        Symbol.ZF => TD.KB_ZF_20191216_TP_EST,
    //        _ => throw new ArgumentOutOfRangeException(nameof(symbol))
    //    };
    //}
}