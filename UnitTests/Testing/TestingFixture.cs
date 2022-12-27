// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using static SquidEyes.UnitTests.Properties.TestData;

namespace SquidEyes.UnitTests;

public class TestingFixture : IDisposable
{
    private readonly Dictionary<Symbol, TickSet> tickSets = new();

    public TestingFixture()
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
            Symbol.BP => KB_BP_20191216_TP_EST,
            Symbol.CL => KB_CL_20191216_TP_EST,
            Symbol.ES => KB_ES_20191216_TP_EST,
            Symbol.EU => KB_EU_20191216_TP_EST,
            Symbol.GC => KB_GC_20191216_TP_EST,
            Symbol.JY => KB_JY_20191216_TP_EST,
            Symbol.NQ => KB_NQ_20191216_TP_EST,
            Symbol.ZB => KB_ZB_20191216_TP_EST,
            Symbol.ZF => KB_ZF_20191216_TP_EST,
            Symbol.ZN => KB_ZN_20191216_TP_EST,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }
}