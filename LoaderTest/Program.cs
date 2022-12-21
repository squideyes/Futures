var folder = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDocuments), "KibotData");

var fileNames = Directory.GetFiles(
    folder, "*.stps", SearchOption.AllDirectories);

Console.ReadKey();