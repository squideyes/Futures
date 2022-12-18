using SquidEyes.Futures.TickSetMaker;

const string KIBOT_DATA = @"C:\ProgramData\Kibot Agent\Data";

var builder = new KibotBuilder(KIBOT_DATA);

builder.Parse();