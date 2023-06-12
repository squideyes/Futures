//// ********************************************************
//// The use of this source code is licensed under the terms
//// of the MIT License (https://opensource.org/licenses/MIT)
//// ********************************************************

//using SquidEyes.Futures;

//namespace SquidEyes.UnitTests;

//public class KnownTests
//{
//    [Fact]
//    public void Known_Assets_Should_BeValid_AfterStaticConstruct()
//    {
//        var symbols = Enum.GetValues<Symbol>();

//        Known.Assets.Count.Should().Be(symbols.Length);

//        foreach (var symbol in symbols)
//        {
//            Known.Assets.Should().ContainKey(symbol);
//            Known.Assets[symbol].Symbol.Should().Be(symbol);
//        }
//    }

//    [Fact]
//    public void Known_GetContract_Returns_ExpectedContract()
//    {
//        var asset = Known.Assets[Symbol.NQ];

//        foreach (var contract in Known.Contracts[asset])
//        {
//            foreach (var tradeDate in contract.TradeDates)
//                Known.GetContract(asset, tradeDate).Should().Be(contract);
//        }
//    }

//    [Theory]
//    [InlineData(Source.Amp, Symbol.BP, "6B")]
//    [InlineData(Source.Amp, Symbol.CL, "CL")]
//    [InlineData(Source.Amp, Symbol.E7, "E7")]
//    [InlineData(Source.Amp, Symbol.ES, "ES")]
//    [InlineData(Source.Amp, Symbol.EU, "6E")]
//    [InlineData(Source.Amp, Symbol.GC, "GC")]
//    [InlineData(Source.Amp, Symbol.J7, "J7")]
//    [InlineData(Source.Amp, Symbol.JY, "6J")]
//    [InlineData(Source.Amp, Symbol.MES, "MES")]
//    [InlineData(Source.Amp, Symbol.MNQ, "MNQ")]
//    [InlineData(Source.Amp, Symbol.NQ, "NQ")]
//    [InlineData(Source.Amp, Symbol.QM, "QM")]
//    [InlineData(Source.Amp, Symbol.QO, "QO")]
//    [InlineData(Source.Amp, Symbol.ZB, "ZB")]
//    [InlineData(Source.Amp, Symbol.ZF, "ZF")]
//    [InlineData(Source.Amp, Symbol.ZN, "ZN")]
//    [InlineData(Source.Kibot, Symbol.BP, "BP")]
//    [InlineData(Source.Kibot, Symbol.CL, "CL")]
//    [InlineData(Source.Kibot, Symbol.ES, "ES")]
//    [InlineData(Source.Kibot, Symbol.EU, "EU")]
//    [InlineData(Source.Kibot, Symbol.ZF, "FV")]
//    [InlineData(Source.Kibot, Symbol.GC, "GC")]
//    [InlineData(Source.Kibot, Symbol.JY, "JY")]
//    [InlineData(Source.Kibot, Symbol.NQ, "NQ")]
//    [InlineData(Source.Kibot, Symbol.ZN, "TY")]
//    [InlineData(Source.Kibot, Symbol.ZB, "US")]
//    public void Known_SymbolAs_Should_ReturnExpectedSymbolAs(
//    Source source, Symbol symbol, string symbolAs)
//    {
//        Known.SymbolAs[source, symbol].Should().Be(symbolAs);
//    }

//    [Fact]
//    public void Known_TradeDates_Should_BeValid_AfterStaticConstruct()
//    {
//        var tradeDates = Known.TradeDates;

//        tradeDates.Count.Should().Be(2502);

//        var holidays = HolidayHelper.GetHolidays();

//        foreach (var date in tradeDates.Keys)
//        {
//            date.IsWeekDay().Should().BeTrue();

//            tradeDates[date].AsDateOnly().Should().Be(date);

//            holidays.Contains(date).Should().BeFalse();
//        }
//    }
//}