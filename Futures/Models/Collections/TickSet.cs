// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;
using System.Collections;
using System.Text;

namespace SquidEyes.Futures.Models;

public class TickSet : IEnumerable<Tick>
{
    private readonly List<Tick> ticks = new();

    public TickSet(Contract contract, DateOnly tradeDate)
    {
        Contract = contract.MayNot().BeNull();
        TradeDate = tradeDate.MayNot().BeDefault();
    }

    public Contract Contract { get; }
    public DateOnly TradeDate { get; }

    public Asset Asset => Contract.Asset;
    public int Count => ticks.Count;

    public Tick this[int index] => ticks[index];

    public void Add(Tick tick) => ticks.Add(tick);

    public override string ToString() => $"{Contract} {TradeDate}";


    public static string GetFileName(Source source,
        Contract contract, TradeDate tradeDate, BarSpec barSpec)
    {
        var sb = new StringBuilder();

        sb.Append(source.ToCode());
        sb.AppendDelimited(contract, '_');
        sb.AppendDelimited(tradeDate.AsDateOnly().ToString("yyyyMMdd"), '_');
        sb.Append("T0001_EST.sts");

        return sb.ToString();
    }

    public static string GetBlobName(Source source,
        Contract contract, TradeDate tradeDate, BarSpec barSpec)
    {
        var sb = new StringBuilder();

        var fileName = GetFileName(source, contract, tradeDate, barSpec);

        sb.Append('/' + source.ToCode());
        sb.AppendDelimited("TickSets", '/');
        sb.AppendDelimited(contract.Asset, '/');
        sb.AppendDelimited(contract, '/');
        sb.AppendDelimited(fileName, '/');

        return sb.ToString();
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Tick> GetEnumerator() => ticks.GetEnumerator();
}