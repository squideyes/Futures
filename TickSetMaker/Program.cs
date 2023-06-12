// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using Microsoft.Extensions.Configuration;
using SquidEyes.Futures;
using SquidEyes.TickSetMaker;

var config = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

//if (!TryGetSource(config, out string source))
//    return PrintHelp();

//if (!TryGetTarget(config, out string target))
//    return PrintHelp();

//if (!TryGetSymbols(config, out List<Symbol> symbols))
//    return PrintHelp();

//KibotParser.ParseAndSave(source, target, symbols);

return 0;

//static bool TryGetSource(IConfiguration config, out string source)
//{
//    source = config["source"]!;

//    if (string.IsNullOrWhiteSpace(source))
//        return false;

//    return Directory.Exists(source);
//}

//static bool TryGetTarget(IConfiguration config, out string target)
//{
//    target = config["target"]!;

//    if (string.IsNullOrWhiteSpace(target))
//    {
//        target = Path.Combine(Environment.GetFolderPath(
//            Environment.SpecialFolder.MyDocuments), "KibotData");
//    }

//    return target.IsRootedPath();
//}

//static bool TryGetSymbols(IConfiguration config, out List<Symbol> symbols)
//{
//    var filter = config["symbols"]!;

//    var kibotSymbols = Known.SymbolAs.GetSymbols(Source.Kibot).ToHashSet();

//    if (string.IsNullOrWhiteSpace(filter))
//    {
//        symbols = kibotSymbols.ToList();

//        return true;
//    }

//    symbols = null!;

//    foreach (var s in filter.Split(','))
//    {
//        if (Enum.TryParse(s, out Symbol symbol))
//        {
//            if (!kibotSymbols.Contains(symbol))
//                return false;

//            symbols ??= new List<Symbol>();

//            symbols.Add(symbol);
//        }
//        else
//        {
//            symbols = null!;

//            return false;
//        }
//    }

//    return symbols != null;
//}

//static int PrintHelp()
//{
//    var symbols = string.Join(',', Enum.GetValues<Symbol>());

//    Console.WriteLine("Converts continuous Kibot Tick-Files to SquidEyes TickSets");
//    Console.WriteLine();
//    Console.WriteLine($"TICKSETMAKER source= [symbols=] [target=]");
//    Console.WriteLine();
//    Console.WriteLine("  source   Folder that contains continuous Kibot Tick-Files");
//    Console.WriteLine($"  symbols  Optional Symbol filter (i.e. ES,ZB)");
//    Console.WriteLine("  target   TickSets folder (default = {MyDocs}\\KibotData)");
//    Console.WriteLine();
//    Console.WriteLine($"Known Symbols: {symbols}");
//    Console.WriteLine("Tick-File CSV Format: MM/dd/yyyy HH:mm:ss.fff,price,volume");

//    return -1;
//}