// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Helpers;
using SquidEyes.Fundamentals;
using System.Collections;
using System.Text;
using System.IO.Compression;

namespace SquidEyes.Futures.Models;

public class TickSet : IEnumerable<Tick>
{
    private readonly List<Tick> ticks = new();

    private readonly Session session;

    private TickSet(Source source, Contract contract, TradeDate tradeDate)
    {
        Source = source;
        Contract = contract;
        Contract = contract;

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
        tick.TickOn.Must().Be(session.IsInSession);
        tick.Must().Be(v => Count == 0 || v.TickOn < ticks.Last().TickOn);

        ticks.Add(tick);
    }

    public string GetFullPath(string basePath)
    {
        basePath.MayNot().BeNullOrWhitespace();

        return Path.Combine(basePath,
            GetPathedFileName(Path.DirectorySeparatorChar));
    }

    public override string ToString() => $"{Contract} {TradeDate}";

    public void SaveTo(Stream stream)
    {
        stream.MayNot().BeNull();

        using var dataStream = new MemoryStream();

        var writer = new BinaryWriter(dataStream, Encoding.UTF8, true);

        writer.Write(Source.ToCode());
        writer.Write(Contract.ToString());
        writer.Write(TradeDate.DayNumber);
        writer.Write(Count);

        foreach (var tick in this)
        {
            writer.Write((int)(tick.TickOn -
                TradeDate.MinTickOn).TotalMilliseconds);

            writer.Write(tick.Price);

            writer.Write(tick.Volume);
        }

        writer.Flush();

        dataStream.Position = 0;

        using var gzip = new GZipStream(
            stream, CompressionLevel.SmallestSize, true);

        dataStream.CopyTo(gzip);
    }

    public void SaveTo(string basePath)
    {
        basePath.MayNot().BeNullOrWhitespace();

        var fullPath = GetFullPath(basePath);

        var directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        using var stream = File.OpenWrite(fullPath);

        SaveTo(stream);
    }

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

    public static TickSet Create(
        Source source, Contract contract, TradeDate tradeDate)
    {
        source.Must().BeEnumValue();

        contract.MayNot().BeNull();

        tradeDate.MayNot().BeDefault()
            .Must().Be(contract.TradeDates.Contains);

        return new TickSet(source, contract, tradeDate);
    }

    public static TickSet LoadFrom(string fullPath)
    {
        using var stream = File.OpenRead(fullPath);

        return LoadFrom(stream);
    }

    public static TickSet LoadFrom(Stream stream)
    {
        stream.MayNot().BeNull();

        using var dataStream = new MemoryStream();

        using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
            gzip.CopyTo(dataStream);

        dataStream.Position = 0;

        var reader = new BinaryReader(dataStream, Encoding.UTF8, true);

        var source = reader.ReadString().ToSource();

        var contract = Contract.Parse(reader.ReadString());

        var tradeDate = KnownTradeDates.From(
            DateOnly.FromDayNumber(reader.ReadInt32()));

        var count = reader.ReadInt32();

        var tickSet = new TickSet(source, contract, tradeDate);

        for (var i = 0; i < count; i++)
        {
            var tickOn = tradeDate.MinTickOn
                .AddMilliseconds(reader.ReadInt32());

            var price = reader.ReadSingle();

            var volume = reader.ReadInt32();

            tickSet.Add(new Tick(tickOn, price, volume));
        }

        return tickSet;
    }

    public static TickSet EmptyFrom(string fullPath)
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