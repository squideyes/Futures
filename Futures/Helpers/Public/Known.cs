using System.Collections.Immutable;
using System.Text;

namespace SquidEyes.Futures;

public static class Known
{
    static Known()
    {
        TradeDates = GetTradeDates();
        Assets = KnownHelper.GetAssets();
        Contracts = GetContracts();
    }

    public static ImmutableSortedDictionary<DateOnly, TradeDate> TradeDates { get; }
    public static IReadOnlyDictionary<Symbol, Asset> Assets { get; }
    public static IReadOnlyDictionary<Asset, List<Contract>> Contracts { get; }

    public static string GetContractsSpecs(bool includeMicros = false)
    {
        var sb = new StringBuilder();

        var yesterday = DateTime.Today;

        foreach (var asset in Assets.Values
            .Where(a => !includeMicros || !a.IsMicro))
        {
            foreach (var contract in Contracts[asset])
            {
                var until = contract.TradeDates.Last().AsDateTime();

                if (until >= yesterday)
                    continue;

                var from = contract.TradeDates.First()
                    .AsDateTime().Add(asset.Market!.Period.From);

                until = until.Add(asset.Market.Period.Until);

                sb.Append(asset.Symbol);
                sb.AppendDelimited(((int)contract.Month).ToString("00"));
                sb.AppendDelimited(contract.Year);
                sb.AppendDelimited(contract);
                sb.AppendDelimited(from.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                sb.AppendDelimited(until.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                sb.AppendLine();
            }
        }

        return sb.ToString();
    }

    private static ImmutableSortedDictionary<DateOnly, TradeDate> GetTradeDates()
    {
        var holidays = HolidayHelper.GetHolidays();

        var dates = new List<DateOnly>();

        for (var date = TradeDate.MinValue;
            date <= TradeDate.MaxValue; date = date.AddDays(1))
        {
            if (date.IsWeekDay() && !holidays.Contains(date))
                dates.Add(date);
        }

        return dates.ToImmutableSortedDictionary(d => d, d => new TradeDate(d));
    }

    private static Dictionary<Asset, List<Contract>> GetContracts()
    {
        static List<Contract> GetContracts(Asset asset)
        {
            var contracts = new List<Contract>();

            for (int year = Contract.MinYear; year <= Contract.MaxYear; year++)
            {
                foreach (var month in asset.Months!)
                    contracts.Add(new Contract(asset, month, year));
            }

            return contracts;
        }

        var contracts = new Dictionary<Asset, List<Contract>>();

        foreach (var asset in Assets.Values)
            contracts.Add(asset, GetContracts(asset));

        return contracts;
    }
}