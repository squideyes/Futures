namespace SquidEyes.Futures.TickSetMaker;

internal static class KibotParser
{
    public static void ParseAndSave(string dataPath, int maxYear, bool overwrite)
    {
        var basePath = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.MyDocuments), "KibotData");

        var lookup = new Dictionary<Symbol, Dictionary<TradeDate, Contract>>();

        int total = 0;
        int count = 0;

        var symbols = Enum.GetValues<Symbol>();

        foreach (var symbol in symbols)
        {
            lookup.Add(symbol, new Dictionary<TradeDate, Contract>());

            var contracts = Known.Contracts[Known.Assets[symbol]];

            var dict = new Dictionary<TradeDate, Contract>();

            foreach (var contract in contracts)
            {
                if (contract.Year > maxYear)
                    break;

                foreach (var tradeDate in contract.TradeDates)
                {
                    total++;

                    lookup[symbol].Add(tradeDate, contract);
                }
            }
        }

        foreach (var symbol in symbols)
        {
            var asset = Known.Assets[symbol];

            var contracts = Known.Contracts[asset];

            var fileName = Path.Join(dataPath, symbol + ".txt");

            using var reader = new StreamReader(fileName);

            TickSet tickSet = null!;

            int tickId = 0;

            string line;

            void NewTickSet(Contract contract, TradeDate tradeDate)
            {
                tickSet = new TickSet(Source.Kibot, contract, tradeDate);

                tickId = 0;
            }

            while ((line = reader.ReadLine()!) != null)
            {
                var fields = line.Split(',');

                var tickOn = TickOn.Parse(fields[0]);

                var tradeDate = tickOn.GetTradeDate(asset);

                if (!lookup[symbol].TryGetValue(tradeDate, out Contract? contract))
                    continue;

                var price = float.Parse(fields[1]);

                var tick = Tick.From(asset, tickId++, tickOn, price);

                if (tickSet == null)
                {
                    NewTickSet(contract, tradeDate);
                }
                else if (tickSet.TradeDate < tradeDate)
                {
                    var prefix = $"{++count:00000} of {total:00000} - ";

                    var fullPath = tickSet.GetFullPath(basePath);

                    if (!overwrite && File.Exists(fullPath))
                    {
                        Console.WriteLine($"{prefix}SKIPPED {tickSet}");
                    }
                    else
                    {
                        tickSet.Save(basePath);

                        Console.WriteLine($"{prefix}SAVED {tickSet} ({tickSet.Count:N0} Ticks)");
                    }

                    NewTickSet(contract, tradeDate);
                }
                else if (tickSet.TradeDate > tradeDate)
                {
                    throw new InvalidDataException("An out of order tick was found!");
                }

                tickSet!.Add(tick);
            }
        }
    }
}
