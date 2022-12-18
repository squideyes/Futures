using System.Collections;
using System.IO.Compression;
using System.Text;

namespace SquidEyes.Futures;

public class TickSet : IEnumerable<Tick>
{
    private readonly List<Tick> ticks = new();

    public TickSet(Source source, Contract contract, TradeDate tradeDate)
    {
        Source = source.Must().BeEnumValue();
        Contract = contract.MayNot().BeNull();
        TradeDate = tradeDate.MayNot().BeDefault();

        TickOn GetTickOn(TradeDate tradeDate,
            Func<Period<TimeSpan>, TimeSpan> getTimeSpan)
        {
            return TickOn.From(tradeDate.AsDateTime()
                .Add(contract.Asset.Market!.Period.From));
        }

        MinTickOn = GetTickOn(Contract.TradeDates.First(), p => p.From);
        MaxTickOn = GetTickOn(Contract.TradeDates.Last(), p => p.Until);
    }

    public Source Source { get; }
    public Contract Contract { get; }
    public TradeDate TradeDate { get; }
    public TickOn MinTickOn { get; }
    public TickOn MaxTickOn { get; }

    public int Count => ticks.Count;

    public Tick this[int index] => ticks[index];

    public string FileName
    {
        get
        {
            var sb = new StringBuilder();

            sb.Append(Source.ToCode());
            sb.AppendDelimited(Contract.Asset, '_');
            sb.AppendDelimited(TradeDate.ToString("yyyyMMdd"), '_');
            sb.Append("_TP_EST.stps");

            return sb.ToString();
        }
    }

    public override string ToString() => FileName;

    public string GetFullPath(string basePath)
    {
        basePath.MayNot().BeNullOrWhitespace();

        return Path.Combine(basePath, Contract.Asset.ToString(), 
            Contract.Year.ToString(), FileName);
    }

    public void Add(Tick tick)
    {
        tick.MayNot().BeDefault();
        tick.TickOn.Must().BeBetween(MinTickOn, MaxTickOn);

        if (ticks.Count > 0)
        {
            var last = ticks.Last();

            if (tick.TickOn < last.TickOn)
                throw new ArgumentOutOfRangeException(nameof(tick));

            if (tick.TickOn == last.TickOn && tick.Price == last.Price)
                return;
        }

        ticks.Add(tick);
    }

    public void Save(Stream stream)
    {
        stream.MayNot().BeNull();

        using var dataStream = new MemoryStream();

        var writer = new BinaryWriter(dataStream, Encoding.UTF8, true);

        writer.Write(Source.ToCode());

        writer.Write(Contract.ToString());

        writer.Write(TradeDate.AsDateOnly().DayNumber);

        writer.Write(ticks.Count);

        foreach (var tick in ticks)
        {
            writer.Write((int)(tick.TickOn.AsDateTime() -
                MinTickOn.AsDateTime()).TotalMilliseconds);

            writer.Write(tick.Price);
        }

        writer.Flush();

        dataStream.Position = 0;

        using var gzip = new GZipStream(
            stream, CompressionLevel.SmallestSize, true);

        dataStream.CopyTo(gzip);
    }

    public void Save(string basePath)
    {
        var fullPath = GetFullPath(basePath);

        fullPath.EnsurePathExists();

        using var stream = File.OpenWrite(fullPath);

        Save(stream);
    }

    public static TickSet From(Stream stream)
    {
        stream.MayNot().BeNull();

        TickSet tickSet;

        using (var dataStream = new MemoryStream())
        {
            using (var gzip = new GZipStream(
                stream, CompressionMode.Decompress))
            {
                gzip.CopyTo(dataStream);
            }

            dataStream.Position = 0;

            var reader = new BinaryReader(dataStream, Encoding.UTF8, true);

            var source = reader.ReadString().ToSource();

            var contract = Contract.Parse(reader.ReadString());

            var tradeDate = TradeDate.From(
                DateOnly.FromDayNumber(reader.ReadInt32()));

            var count = reader.ReadInt32();

            int tickId = 0;

            tickSet = new TickSet(source, contract, tradeDate);

            for (var i = 0; i < count; i++)
            {
                var tickOn = TickOn.From(tickSet.MinTickOn
                    .AsDateTime().AddMilliseconds(reader.ReadInt32()));

                var price = reader.ReadSingle();

                tickSet.Add(new Tick(tickId++, tickOn, price));
            }
        }

        return tickSet;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public IEnumerator<Tick> GetEnumerator() => ticks.GetEnumerator();
}