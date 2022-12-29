// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using TD = SquidEyes.Testing.Properties.TestData;

namespace SquidEyes.Testing;

public class TestData : IDisposable
{
    private readonly Dictionary<Symbol, TickSet> tickSets = new();

    public TestData()
    {
        foreach (var symbol in Known.SymbolAs.GetSymbols(Source.Kibot))
        {
            var asset = Known.Assets[symbol];

            using var stream = new MemoryStream(GetBytes(symbol));

            var tickSet = TickSet.From(stream);

            tickSets.Add(symbol, tickSet);
        }
    }

    public TradeDate TradeDate { get; } =
        TradeDate.From(new DateOnly(2021, 12, 13));

    public TickSet GetTickSet(Symbol symbol) => tickSets[symbol];

    public void Dispose() => GC.SuppressFinalize(this);

    private static byte[] GetBytes(Symbol symbol)
    {
        return symbol switch
        {
            Symbol.BP => TD.KB_BP_20191216_TP_EST,
            Symbol.CL => TD.KB_CL_20191216_TP_EST,
            Symbol.ES => TD.KB_ES_20191216_TP_EST,
            Symbol.EU => TD.KB_EU_20191216_TP_EST,
            Symbol.GC => TD.KB_GC_20191216_TP_EST,
            Symbol.JY => TD.KB_JY_20191216_TP_EST,
            Symbol.NQ => TD.KB_NQ_20191216_TP_EST,
            Symbol.ZB => TD.KB_ZB_20191216_TP_EST,
            Symbol.ZF => TD.KB_ZF_20191216_TP_EST,
            Symbol.ZN => TD.KB_ZN_20191216_TP_EST,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }
}