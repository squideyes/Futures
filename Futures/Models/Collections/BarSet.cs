// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;
using System.Collections;
using System.Text;

namespace SquidEyes.Futures.Models;

public class BarSet : IEnumerable<Bar>
{
    private readonly List<Bar> bars = new();

    public BarSet(Source source, BarSpec barSpec,
        Contract contract, TradeDate tradeDate)
    {
        Source = source.Must().BeEnumValue();
        BarSpec = barSpec.MayNot().BeDefault();
        Contract = contract.MayNot().BeNull();
        TradeDate = tradeDate.MayNot().BeDefault();
    }

    public Source Source { get; }
    public Contract Contract { get; }
    public TradeDate TradeDate { get; }
    public BarSpec BarSpec { get; }

    public int Count => bars.Count;
    public Bar this[int index] => bars[1];

    public string FileName =>
        GetFileName(Source, Contract, TradeDate, BarSpec);

    public string BlobName =>
        GetFileName(Source, Contract, TradeDate, BarSpec);

    public void Add(Bar bar)
    {
        //bar.MayNot().BeNull();

        //if (!bar.IsValid(Contract.Asset, BarSpec))
        //    throw new ArgumentOutOfRangeException(nameof(bar));

        //if (bars.Any() && bar.OpenOn < bars.Last().OpenOn)
        //    throw new ArgumentOutOfRangeException(nameof(bar));

        //bars.Add(bar);
    }

    public static string GetFileName(Source source,
        Contract contract, TradeDate tradeDate, BarSpec barSpec)
    {
        var sb = new StringBuilder();

        sb.Append(source.ToCode());
        sb.AppendDelimited(contract, '_');
        sb.AppendDelimited(tradeDate.AsDateOnly().ToString("yyyyMMdd"), '_');
        sb.AppendDelimited(barSpec, '_');
        sb.Append("EST.sbs");

        return sb.ToString();
    }

    public static string GetBlobName(Source source,
        Contract contract, TradeDate tradeDate, BarSpec barSpec)
    {
        var sb = new StringBuilder();

        var fileName = GetFileName(source, contract, tradeDate, barSpec);

        sb.Append('/' + source.ToCode());
        sb.AppendDelimited("BarSets", '/');
        sb.AppendDelimited(contract.Asset, '/');
        sb.AppendDelimited(contract, '/');
        sb.AppendDelimited(barSpec, '/');
        sb.AppendDelimited(fileName, '/');

        return sb.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Bar> GetEnumerator() => bars.GetEnumerator();
}