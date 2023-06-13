// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.Futures.Models;
using SquidEyes.Futures.Helpers;

namespace SquidEyes.TickSetMaker;
internal static class KibotParser
{
    public static void ParseAndSave(string source, string target, List<string> symbols)
    {
        var total = GetTotal(symbols);

        int count = 0;

        foreach (var symbol in symbols)
        {
            var asset = KnownAssets.Get(symbol);

            var kibotSymbol = KnownSymbols.Get(Source.KibotHistory, asset);

            var fileName = Path.Join(source, kibotSymbol + ".txt");

            using var reader = new StreamReader(fileName);

            TickSet tickSet = null!;

            string line;

            void NewTickSet(Contract contract, TradeDate tradeDate) =>
                tickSet = new TickSet(Source.KibotHistory, contract, tradeDate);

            while ((line = reader.ReadLine()!) != null)
            {
                var fields = line.Split(',');

                var tickOn = DateTime.Parse(fields[0]);

                var date = tickOn.ToPotentialTradeDateValue();

                if (!KnownTradeDates.TryGetTradeDate(date, out TradeDate tradeDate))
                    continue;

                if (!tradeDate.IsTickOn(tickOn))
                    continue;

                var contract = asset.GetContract(tradeDate);

                var price = float.Parse(fields[1]);

                var volume = int.Parse(fields[2]);

                var tick = Tick.From(tickOn, price, volume);

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
                        tickSet.SaveTo(target);

                        Console.WriteLine($"{prefix}SAVED {tickSet} ({tickSet.Count:N0} Ticks)");
                    }

                    NewTickSet(contract, tradeDate);
                }
                else if (tickSet.TradeDate > tradeDate)
                {
                    throw new InvalidDataException(
                        "An out of order tick was found!");
                }

                tickSet!.Add(tick);
            }
        }
    }

    private static int GetTotal(List<string> symbols)
    {
        int total = 0;

        var maxDate = DateOnly.FromDateTime(
            DateTime.UtcNow.ToEasternFromUtc().Date).AddDays(-1);

        foreach (var symbol in symbols)
        {
            var contracts = KnownAssets.Get(symbol).Contracts;

            foreach (var contract in contracts)
            {
                if (contract.TradeDates.Last().AsDateOnly() >= maxDate)
                    break;

                foreach (var tradeDate in contract.TradeDates)
                    total++;
            }
        }

        return total;
    }
}