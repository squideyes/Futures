using SquidEyes.Futures;
using static SquidEyes.UnitTests.Properties.TestData;

namespace SquidEyes.UnitTests;

public class TestingFixture : IDisposable
{
    private readonly Dictionary<Symbol, TickSet> tickSets = new();

    public TestingFixture()
    {
        foreach (var symbol in Enum.GetValues<Symbol>())
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
            Symbol.CL => KB_CL_20211213_TP_EST,
            Symbol.ES => KB_ES_20211213_TP_EST,
            Symbol.EU => KB_EU_20211213_TP_EST,
            Symbol.GC => KB_GC_20211213_TP_EST,
            Symbol.JY => KB_JY_20211213_TP_EST,
            Symbol.NQ => KB_NQ_20211213_TP_EST,
            Symbol.TY => KB_TY_20211213_TP_EST,
            Symbol.US => KB_US_20211213_TP_EST,
            _ => throw new ArgumentOutOfRangeException(nameof(symbol))
        };
    }
}