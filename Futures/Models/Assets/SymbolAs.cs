// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class SymbolAs
{
    private readonly Dictionary<(Source Source, Symbol Symbol), string> datas = new();

    public SymbolAs()
    {
        datas.Add((Source.Amp, Symbol.BP), "6B");
        datas.Add((Source.Amp, Symbol.CL), "CL");
        datas.Add((Source.Amp, Symbol.E7), "E7");
        datas.Add((Source.Amp, Symbol.ES), "ES");
        datas.Add((Source.Amp, Symbol.EU), "6E");
        datas.Add((Source.Amp, Symbol.GC), "GC");
        datas.Add((Source.Amp, Symbol.J7), "J7");
        datas.Add((Source.Amp, Symbol.JY), "6J");
        datas.Add((Source.Amp, Symbol.MES), "MES");
        datas.Add((Source.Amp, Symbol.MNQ), "MNQ");
        datas.Add((Source.Amp, Symbol.NQ), "NQ");
        datas.Add((Source.Amp, Symbol.QM), "QM");
        datas.Add((Source.Amp, Symbol.QO), "QO");
        datas.Add((Source.Amp, Symbol.ZB), "ZB");
        datas.Add((Source.Amp, Symbol.ZF), "ZF");
        datas.Add((Source.Amp, Symbol.ZN), "ZN");

        datas.Add((Source.Kibot, Symbol.BP), "BP");
        datas.Add((Source.Kibot, Symbol.CL), "CL");
        datas.Add((Source.Kibot, Symbol.ES), "ES");
        datas.Add((Source.Kibot, Symbol.EU), "EU");
        datas.Add((Source.Kibot, Symbol.ZF), "FV");
        datas.Add((Source.Kibot, Symbol.GC), "GC");
        datas.Add((Source.Kibot, Symbol.JY), "JY");
        datas.Add((Source.Kibot, Symbol.NQ), "NQ");
        datas.Add((Source.Kibot, Symbol.ZN), "TY");
        datas.Add((Source.Kibot, Symbol.ZB), "US");
    }

    public string this[Source source, Symbol symbol] =>  datas[(source, symbol)];

    public List<Symbol> GetSymbols(Source source)
    {
        return datas.Where(kv => kv.Key.Source == source)
            .Select(kv => kv.Key.Symbol).ToList();
    }
}