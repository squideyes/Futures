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

    private readonly Session session;

    public TickSet(Source source,
        Contract contract, TradeDate tradeDate)
    {
        Source = source.Must().BeEnumValue();

        Contract = contract.MayNot().BeNull();

        TradeDate = tradeDate.MayNot().BeDefault()
            .Must().Be(Contract.TradeDates.Contains);

        session = new Session(tradeDate, DataSpan.Day);
    }

    public Source Source { get; }
    public Contract Contract { get; }
    public TradeDate TradeDate { get; }

    public Asset Asset => Contract.Asset;

    public int Count => ticks.Count;

    public string FileName
    {
        get
        {
            var sb = new StringBuilder();

            sb.Append(Source.ToCode());
            sb.AppendDelimited(Contract.Asset, '_');
            sb.AppendDelimited(TradeDate
                .AsDateOnly().ToString("yyyyMMdd"), '_');
            sb.Append("_TICK_EST.stpvs");

            return sb.ToString();
        }
    }

    public string BlobName => "/" + GetPathedFileName('/');

    public Tick this[int index] => ticks[index];

    public void Add(Tick tick)
    {
        tick.MayNot().BeDefault();

        if (!session.IsInSession(tick.TickOn))
            throw new ArgumentOutOfRangeException(nameof(tick));

        if (Count > 0 && tick.TickOn < ticks.Last().TickOn)
            throw new ArgumentOutOfRangeException(nameof(tick));

        ticks.Add(tick);
    }

    public string GetFullPath(string basePath)
    {
        basePath.MayNot().BeNullOrWhitespace();

        return Path.Combine(basePath, 
            GetPathedFileName(Path.DirectorySeparatorChar));
    }

    public override string ToString() => $"{Contract} {TradeDate}";

    private string GetPathedFileName(char delimiter)
    {
        var sb = new StringBuilder();

        var fileName = FileName;

        sb.Append(Source.ToCode());
        sb.AppendDelimited("TICKSET", delimiter);
        sb.AppendDelimited(Contract.Asset, delimiter);
        sb.AppendDelimited(fileName, delimiter);

        return sb.ToString();
    }

    public static TickSet FromFileName(string fullPath)
    {
        var nameOnly = Path.GetFileNameWithoutExtension(fullPath);

        var fields = nameOnly.Split('_');

        fields.Length.Must().Be(5);

        var source = fields[0].ToSource();

        var asset = KnownAssets.Get(fields[1]);
        
        var tradeDate = KnownTradeDates.From(
            DateOnly.ParseExact(fields[2], "yyyyMMdd", null));

        var contract = asset.GetContract(tradeDate);

        fields[3].Must().Be("TICK");

        fields[4].Must().Be("EST");

        Path.GetExtension(fullPath).Must().Be(".stpvs");

        return new TickSet(source, contract, tradeDate);
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Tick> GetEnumerator() => ticks.GetEnumerator();
}