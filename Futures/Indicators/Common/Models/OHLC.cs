// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

//namespace SquidEyes.Futures;

//internal class OHLC : IOHLC
//{
//    public OHLC(TickOn closeOn, float price)
//    {
//        CloseOn = closeOn;
//        Open = High = Low = Close = price;
//    }

//    public TickOn CloseOn { get; set; }
//    public float Open { get; set; }
//    public float High { get; set; }
//    public float Low { get; set; }
//    public float Close { get; set; }

//    public void Adjust(Tick tick)
//    {
//        if (tick.Price > High)
//            High = tick.Price;

//        if (tick.Price < Low)
//            Low = tick.Price;

//        Close = tick.Price;
//    }
//}