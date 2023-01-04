using System.Collections;

namespace SquidEyes.Futures;

public class CandleSet : IEnumerable<Candle>
{
    private readonly List<Candle> candles = new();

    private CandleSet(Contract contract, TradeDate tradeDate)
    {
        Contract = contract;
        TradeDate = tradeDate;
    }

    public Contract Contract { get; }
    public TradeDate TradeDate { get; }

    public int Count => candles.Count;

    public Candle this[int index] => candles[index];

    public void Add(Candle candle)
    {
        candle.CloseOn.Must(v => candles.Count > 0)
            .Be(v => v >= candles[^1].CloseOn);

        candles.Add(candle);
    }

    public static CandleSet Create(
        Contract contract, TradeDate tradeDate)
    {
        contract.MayNot().BeNull();
        tradeDate.MayNot().BeDefault();

        return new CandleSet(contract, tradeDate);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Candle> GetEnumerator() => candles.GetEnumerator();
}
