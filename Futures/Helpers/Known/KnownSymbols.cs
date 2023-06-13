// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Helpers;

public static class KnownSymbols
{
    private static readonly Dictionary
        <(Source Source, string Symbol), string> datas = new();

    static KnownSymbols()
    {
        datas.Add((Source.RithmicPaper, "BP"), "BP");
        datas.Add((Source.RithmicPaper, "CL"), "CL");
        datas.Add((Source.RithmicPaper, "E7"), "E7");
        datas.Add((Source.RithmicPaper, "ES"), "ES");
        datas.Add((Source.RithmicPaper, "EU"), "EU");
        datas.Add((Source.RithmicPaper, "GC"), "GC");
        datas.Add((Source.RithmicPaper, "J7"), "J7");
        datas.Add((Source.RithmicPaper, "JY"), "JY");
        datas.Add((Source.RithmicPaper, "MES"), "MES");
        datas.Add((Source.RithmicPaper, "MNQ"), "MNQ");
        datas.Add((Source.RithmicPaper, "NQ"), "NQ");
        datas.Add((Source.RithmicPaper, "QM"), "QM");
        datas.Add((Source.RithmicPaper, "QO"), "QO");
        datas.Add((Source.RithmicPaper, "ZB"), "ZB");
        datas.Add((Source.RithmicPaper, "ZN"), "ZN");
        datas.Add((Source.RithmicPaper, "ZF"), "ZF");

        datas.Add((Source.RithmicLive, "BP"), "BP");
        datas.Add((Source.RithmicLive, "CL"), "CL");
        datas.Add((Source.RithmicLive, "E7"), "E7");
        datas.Add((Source.RithmicLive, "ES"), "ES");
        datas.Add((Source.RithmicLive, "EU"), "EU");
        datas.Add((Source.RithmicLive, "GC"), "GC");
        datas.Add((Source.RithmicLive, "J7"), "J7");
        datas.Add((Source.RithmicLive, "JY"), "JY");
        datas.Add((Source.RithmicLive, "MES"), "MES");
        datas.Add((Source.RithmicLive, "MNQ"), "MNQ");
        datas.Add((Source.RithmicLive, "NQ"), "NQ");
        datas.Add((Source.RithmicLive, "QM"), "QM");
        datas.Add((Source.RithmicLive, "QO"), "QO");
        datas.Add((Source.RithmicLive, "ZB"), "ZB");
        datas.Add((Source.RithmicLive, "ZN"), "ZN");
        datas.Add((Source.RithmicLive, "ZF"), "ZF");

        datas.Add((Source.KibotHistory, "BP"), "BP");
        datas.Add((Source.KibotHistory, "CL"), "CL");
        datas.Add((Source.KibotHistory, "ES"), "ES");
        datas.Add((Source.KibotHistory, "EU"), "EU");
        datas.Add((Source.KibotHistory, "ZF"), "FV");
        datas.Add((Source.KibotHistory, "GC"), "GC");
        datas.Add((Source.KibotHistory, "JY"), "JY");
        datas.Add((Source.KibotHistory, "NQ"), "NQ");
        datas.Add((Source.KibotHistory, "ZN"), "TY");
        datas.Add((Source.KibotHistory, "ZB"), "US");
    }

    public static bool Contains(Source source, string symbol) =>
         datas.ContainsKey((source, symbol));

    public static string Get(Source source, Asset asset) =>
       datas[(source, asset.Symbol)];

    public static List<string> GetAll(Source source)
    {
        return datas.Where(kv => kv.Key.Source == source)
            .Select(kv => kv.Key.Symbol).ToList();
    }
}