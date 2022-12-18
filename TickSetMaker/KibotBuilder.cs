using SquidEyes.Futures;

namespace SquidEyes.Futures.TickSetMaker;

internal class KibotBuilder
{
    private readonly string dataPath;

    public KibotBuilder(string dataPath)
    {
        this.dataPath = dataPath;
    }

    public void Parse()
    {
        var fileNames = Directory.GetFiles(dataPath);

        var basePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), "KibotData");

        foreach (var fileName in fileNames)
        {
            var noExt = Path.GetFileNameWithoutExtension(fileName);

            if (!Enum.TryParse(noExt, true, out Symbol symbol))
                continue;

            var asset = Known.Assets[symbol];

            var contracts = Known.Contracts[asset];

            var lookup = new Dictionary<TradeDate, Contract>();

            foreach(var contract in contracts)
            {
                foreach(var tradeDate in contract.TradeDates)
                    lookup.Add(tradeDate, contract);
            }

            using var reader = new StreamReader(fileName);

            TickSet tickSet = null!;
            var tickId = 0;

            string line;

            while ((line = reader.ReadLine()!) != null)
            {
                var fields = line.Split(',');

                var tickOn = TickOn.Parse(fields[0]);

                var tradeDate = tickOn.GetTradeDate(asset);

                if (!lookup.TryGetValue(tradeDate, out Contract? contract))
                    continue;

                var price = float.Parse(fields[1]);

                var tick = Tick.From(asset, tickId++, tickOn, price);

                if (tickSet == null)
                {
                    tickSet = new TickSet(Source.Kibot, contract, tradeDate);
                }
                else if (tickSet.TradeDate < tradeDate)
                {
                    tickSet.Save(basePath);

                    Console.WriteLine(
                        $"SAVED {tickSet} ({tickSet.Count:N0} Ticks)");

                    tickSet = new TickSet(Source.Kibot, contract, tradeDate);
                }
                else if (tickSet.TradeDate > tradeDate)
                {
                    throw new InvalidDataException(
                        "An out of order tick was found!");
                }

                tickSet.Add(tick);
            }
        }
    }
}
