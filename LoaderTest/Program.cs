// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using SquidEyes.Testing;

var folder = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDocuments), "KibotData");

var fileNames = Directory.GetFiles(
    folder, "*.stps", SearchOption.AllDirectories);

string prefix = null!;

try
{
    int count = 0;
    var startedOn = DateTime.UtcNow;

    foreach (var fileName in fileNames)
    {
        prefix = $"{++count:0000} of {fileNames.Length:0000} - ";

        var source = Roundtrip(fileName);

        Console.WriteLine(
            $"{prefix}ISVALID: {source} ({source.Count:N0} ticks)");
    }

    var elapsed = DateTime.UtcNow - startedOn;

    var average = elapsed / fileNames.Length;

    Console.WriteLine($"ELAPSED: {elapsed}, AVERAGE: {average}");
}
catch (Exception error)
{
    Console.WriteLine($"{prefix} - ERROR: {error.Message}");
}

static TickSet Roundtrip(string fileName)
{
    using var stream = File.OpenRead(fileName);

    var source = TickSet.From(stream);

    var target = new TickSet(
        source.Source, source.Contract, source.TradeDate);

    foreach (var tick in source)
        target.Add(tick);

    TestingHelper.AssertSourceMatchesTarget(source, target);

    return source;
}