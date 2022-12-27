// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

var folder = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.MyDocuments), "KibotData");

var fileNames = Directory.GetFiles(
    folder, "*.stps", SearchOption.AllDirectories);

Console.ReadKey();