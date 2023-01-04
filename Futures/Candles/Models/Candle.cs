// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;
using System.Text;

namespace SquidEyes.Futures;

public class Candle : IEquatable<Candle>, IOHLC
{
    public Candle(Asset asset, int candleId, TickOn closeOn, float open, 
        float high, float low, float close, bool isClosed = false)
    {
        asset.MayNot().BeNull();

        CandleId = candleId.Must().BePositiveOrZero();

        CloseOn = closeOn.MayNot().BeDefault();

        Low = low.Must().Be(asset.IsPrice);

        High = high.Must().Be(asset.IsPrice);

        Open = open.Must().Be(
            v => asset.IsPrice(v) && v >= Low && v <= High);

        Close = close.Must().Be(
            v => asset.IsPrice(v) && v >= Low && v <= High);

        Mid = asset.Round((Low + High) / 2.0f);

        IsClosed = isClosed;
    }

    public int CandleId { get; }
    public TickOn CloseOn { get; }
    public float Open { get; }

    public float High { get; private set; }
    public float Low { get; private set; }
    public float Close { get; private set; }
    public float Mid { get; private set; }
    public bool IsClosed { get; private set; }

    public Trend Trend
    {
        get
        {
            if (Open < Close)
                return Trend.Up;
            else if (Open > Close)
                return Trend.Down;
            else
                return Trend.None;
        }
    }

    public bool Equals(Candle? other)
    {
        return other is not null
            && CandleId.Equals(other.CandleId)
            && CloseOn.Equals(other.CloseOn)
            && Open.Equals(other.Open)
            && High.Equals(other.High)
            && Low.Equals(other.Low)
            && Close.Equals(other.Close);
    }

    public override bool Equals(object? other) =>
        other is Candle candle && Equals(candle);

    override public int GetHashCode()
    {
        return CandleId.GetHashCode()
            ^ CloseOn.GetHashCode()
            ^ Open.GetHashCode()
            ^ High.GetHashCode()
            ^ Low.GetHashCode()
            ^ Close.GetHashCode();
    }

    public string ToCsvString(Asset asset)
    {
        var sb = new StringBuilder();

        sb.Append(CandleId.ToString("0000"));
        sb.AppendDelimited(CloseOn);
        sb.AppendDelimited(asset.Format(Open));
        sb.AppendDelimited(asset.Format(High));
        sb.AppendDelimited(asset.Format(Low));
        sb.AppendDelimited(asset.Format(Close));

        return sb.ToString();
    }

    public override string ToString() =>
        $"{CandleId},{CloseOn},{Open},{High},{Low},{Close}";

    public static bool operator ==(Candle lhs, Candle rhs) =>
        (lhs is null && rhs is null) || lhs is not null && lhs.Equals(rhs);

    public static bool operator !=(Candle lhs, Candle rhs) =>
        !(lhs == rhs);
}