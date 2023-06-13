using SquidEyes.Futures.Models;
using System.IO.Compression;
using System.Text;
using SquidEyes.Fundamentals;

namespace SquidEyes.Futures.Helpers;

public static class TickSetExtenders
{
    public static void SaveTo(this TickSet tickSet, Stream stream)
    {
        tickSet.MayNot().BeNull();
        stream.MayNot().BeNull();

        using var dataStream = new MemoryStream();

        var writer = new BinaryWriter(dataStream, Encoding.UTF8, true);

        writer.Write(tickSet.Source.ToCode());
        writer.Write(tickSet.Contract.ToString());
        writer.Write(tickSet.TradeDate.DayNumber);
        writer.Write(tickSet.Count);

        foreach (var tick in tickSet)
        {
            writer.Write((int)(tick.TickOn -
                tickSet.TradeDate.MinTickOn).TotalMilliseconds);

            writer.Write(tick.Price);

            writer.Write(tick.Volume);
        }

        writer.Flush();

        dataStream.Position = 0;

        using var gzip = new GZipStream(
            stream, CompressionLevel.SmallestSize, true);

        dataStream.CopyTo(gzip);
    }

    public static void SaveTo(this TickSet tickSet, string basePath)
    {
        var fullPath = tickSet.GetFullPath(basePath);

        var directory = Path.GetDirectoryName(fullPath);

        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory!);

        using var stream = File.OpenWrite(fullPath);

        tickSet.SaveTo(stream);
    }

    public static void LoadFrom(string fullPath)
    {
        var tickSet = TickSet.FromFileName(fullPath);

        using var stream = File.OpenRead(fullPath);

        tickSet.LoadFrom(stream);
    }

    public static void LoadFrom(this TickSet tickSet, Stream stream)
    {
        stream.MayNot().BeNull();

        using var dataStream = new MemoryStream();

        using (var gzip = new GZipStream(stream, CompressionMode.Decompress))
            gzip.CopyTo(dataStream);

        dataStream.Position = 0;

        var reader = new BinaryReader(dataStream, Encoding.UTF8, true);

        var source = reader.ReadString().ToSource()
            .Must().Be(v => v == tickSet.Source);

        var contract = Contract.Parse(reader.ReadString())
            .Must().Be(v => v == tickSet.Contract);

        var tradeDate = KnownTradeDates.From(
            DateOnly.FromDayNumber(reader.ReadInt32()));

        tradeDate.Must().Be(v => v == tickSet.TradeDate);

        var count = reader.ReadInt32();

        for (var i = 0; i < count; i++)
        {
            var tickOn = tradeDate.MinTickOn
                .AddMilliseconds(reader.ReadInt32());

            var price = reader.ReadSingle();

            var volume = reader.ReadInt32();

            tickSet.Add(new Tick(tickOn, price, volume));
        }
    }
}