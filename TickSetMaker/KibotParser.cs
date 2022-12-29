// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.TickSetMaker;

using Lookup = Dictionary<Symbol, Dictionary<TradeDate, Contract>>;

internal static class KibotParser
{
    public static void ParseAndSave(
        string source, string target, List<Symbol> symbols)
    {
        var (lookup, total) = GetLookupAndTotal(symbols);

        int count = 0;

        foreach (var symbol in symbols)
        {
            var asset = Known.Assets[symbol];

            var contracts = Known.Contracts[asset];

            var kibotSymbol = Known.SymbolAs[Source.Kibot, symbol];

            var fileName = Path.Join(source, kibotSymbol + ".txt");

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

                if (!TickOn.TryParse(fields[0], out TickOn tickOn))
                    continue;

                var tradeDate = tickOn.AsTradeDate();

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
                    var prefix = $"{++count:0000} of {total:0000} - ";

                    var fullPath = tickSet.GetFullPath(target);

                    if (File.Exists(fullPath))
                    {
                        Console.WriteLine($"{prefix}SKIPPED {tickSet}");
                    }
                    else
                    {
                        tickSet.Save(target);

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

    private static (Lookup, int) GetLookupAndTotal(List<Symbol> symbols)
    {
        var lookup = new Lookup();

        int total = 0;

        var yesterday = TradeDate.From(DateOnly.FromDateTime(
            DateTime.UtcNow.ToEasternFromUtc().AddDays(-1)));

        foreach (var symbol in symbols)
        {
            lookup.Add(symbol, new Dictionary<TradeDate, Contract>());

            var contracts = Known.Contracts[Known.Assets[symbol]];

            var dict = new Dictionary<TradeDate, Contract>();

            foreach (var contract in contracts)
            {
                if (contract.TradeDates.Last() > yesterday)
                    break;

                foreach (var tradeDate in contract.TradeDates)
                {
                    total++;

                    lookup[symbol].Add(tradeDate, contract);
                }
            }
        }

        return (lookup, total);
    }
}