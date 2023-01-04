// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;
using static SquidEyes.Futures.KdsBarKind;
using static System.Math;

namespace SquidEyes.Futures;

public class KdsIndicator : IEnumerable<KdsResult>
{
    private const int BARS_TO_PRIME = 20;

    private readonly Series<double> preliminaryTrends = new();
    private readonly Series<double> trends = new();
    private readonly SlidingBuffer<Candle> candles = new(3, true);

    private readonly KdsSettings settings;
    private readonly Asset asset;
    private readonly IBasicIndicator fastMa;
    private readonly IBasicIndicator slowMa;
    private readonly SmaIndicator atrSma;
    private readonly StdDevIndicator stdDev;
    private readonly SlidingBuffer<KdsResult> results;

    private bool init = false;

    public KdsIndicator(Asset asset, KdsSettings settings, int bufferSize = 100)
    {
        this.asset = asset ?? throw new ArgumentNullException(nameof(asset));

        if (settings == null)
            throw new ArgumentNullException(nameof(settings));

        settings.Validate();

        this.settings = settings;

        results = new SlidingBuffer<KdsResult>(bufferSize, true);

        static IBasicIndicator GetMA(MaKind kind, int period)
        {
            return kind switch
            {
                MaKind.SMA => new SmaIndicator(period),
                MaKind.EMA => new EmaIndicator(period),
                _ => throw new ArgumentOutOfRangeException(nameof(kind))
            };
        }

        fastMa = GetMA(settings.FastMA, settings.FastPeriod);
        slowMa = GetMA(settings.SlowMA, settings.SlowPeriod);
        atrSma = new SmaIndicator(settings.AvgTbtrPeriod);

        stdDev = new StdDevIndicator(settings.StdDevPeriod);
    }

    public KdsResult this[int index] => results[index];

    public KdsResult AddAndCalc(Candle candle)
    {
        candles.Add(candle);

        var result = new KdsResult();

        results.Add(result);

        fastMa.AddAndCalc(candle);
        slowMa.AddAndCalc(candle);

        preliminaryTrends.Add();
        trends.Add();

        void UpdateTrueRangeDouble(double value)
        {
            var ohlc = new OHLC(candle.CloseOn, (float)value);

            atrSma.AddAndCalc(ohlc);

            this.stdDev.AddAndCalc(ohlc);
        }

        double Open(int index) => candles[index].Open;
        double High(int index) => candles[index].High;
        double Low(int index) => candles[index].Low;
        double Close(int index) => candles[index].Close;

        if (candle.CandleId < BARS_TO_PRIME)
        {
            result.WarningLine = Close(0);
            result.StopDot1 = Close(0);
            result.StopDot2 = Close(0);
            result.StopDot3 = Close(0);
            result.BarKind = None;
            result.WarningKind = KdsDotKind.None;

            if (candle.CandleId < 1)
            {
                UpdateTrueRangeDouble(High(0) - Low(0));
            }
            else if (candle.CandleId < 2)
            {
                var high1 = High(1);
                var low1 = Low(1);

                UpdateTrueRangeDouble(Max(high1, High(0)) - Min(low1, Low(0)));

                preliminaryTrends[0] = 1.0;

                trends[0] = 0.0;
            }
            else
            {
                preliminaryTrends[2] = 0.0;
                var high1 = Max(High(1), Close(2));
                var low1 = Min(Low(1), Close(2));

                UpdateTrueRangeDouble(Max(high1, High(0)) - Min(low1, Low(0)));

                if (fastMa[0].Result > slowMa[0].Result)
                    preliminaryTrends[0] = 1.0;
                else if (fastMa[0].Result < fastMa[0].Result)
                    preliminaryTrends[0] = -1.0;
                else
                    preliminaryTrends[0] = preliminaryTrends[1];

                trends[0] = 0.0;
            }

            return results[0];
        }

        var avgTbtr = Max(asset.OneTick, atrSma[0].Result);
        var stdDev = this.stdDev[0].Result;
        var offset0 = settings.AvgTbtrMultiplier * avgTbtr;
        var offset1 = offset0 + settings.StdDevMultiplier1 * stdDev;
        var offset2 = offset0 + settings.StdDevMultiplier2 * stdDev;
        var offset3 = offset0 + settings.StdDevMultiplier3 * stdDev;

        if (preliminaryTrends[1] > 0.5)
        {
            var anchor = 0.0;

            if (settings.AnchorMode == KdsAnchorMode.HighLow)
                anchor = Max(High(1), High(2));
            else if (settings.AnchorMode == KdsAnchorMode.Close)
                anchor = Close(1);
            else if (settings.AnchorMode == KdsAnchorMode.Typical)
                anchor = (Max(High(1), High(2)) + Min(Low(1), Low(2)) + Close(1)) / 3.0;
            else if (settings.AnchorMode == KdsAnchorMode.Median)
                anchor = (Max(High(1), High(2)) + Min(Low(1), Low(2))) / 2.0;

            if (settings.TrailingStop && preliminaryTrends[2] > 0.5 && init)
            {
                result.WarningLine = Max(results[1].WarningLine, anchor - offset0);
                result.StopDot1 = Max(results[1].StopDot1, anchor - offset1);
                result.StopDot2 = Max(results[1].StopDot2, anchor - offset2);
                result.StopDot3 = Max(results[1].StopDot3, anchor - offset3);
            }
            else
            {
                result.WarningLine = anchor - offset0;
                result.StopDot1 = anchor - offset1;
                result.StopDot2 = anchor - offset2;
                result.StopDot3 = anchor - offset3;
            }

            if (preliminaryTrends[2] < -0.5)
                result.WarningKind = KdsDotKind.None;
            else
                result.WarningKind = KdsDotKind.Up;
        }
        else if (preliminaryTrends[1] < -0.5)
        {
            var anchor = 0.0;

            if (settings.AnchorMode == KdsAnchorMode.HighLow)
                anchor = Min(Low(1), Low(2));
            else if (settings.AnchorMode == KdsAnchorMode.Close)
                anchor = Close(1);
            else if (settings.AnchorMode == KdsAnchorMode.Typical)
                anchor = (Max(High(1), High(2)) + Min(Low(1), Low(2)) + Close(1)) / 3.0;
            else if (settings.AnchorMode == KdsAnchorMode.Median)
                anchor = (Max(High(1), High(2)) + Min(Low(1), Low(2))) / 2.0;

            if (settings.TrailingStop && preliminaryTrends[2] < -0.5 && init)
            {
                result.WarningLine = Min(results[1].WarningLine, anchor + offset0);
                result.StopDot1 = Min(results[1].StopDot1, anchor + offset1);
                result.StopDot2 = Min(results[1].StopDot2, anchor + offset2);
                result.StopDot3 = Min(results[1].StopDot3, anchor + offset3);
            }
            else
            {
                result.WarningLine = anchor + offset0;
                result.StopDot1 = anchor + offset1;
                result.StopDot2 = anchor + offset2;
                result.StopDot3 = anchor + offset3;
            }

            if (preliminaryTrends[2] > 0.5)
                result.WarningKind = KdsDotKind.None;
            else
                result.WarningKind = KdsDotKind.Down;
        }

        if (candle.CandleId >= BARS_TO_PRIME)
        {
            if (preliminaryTrends[1] > 0.5)
            {
                if (Open(0) < Close(0))
                    results[0].BarKind = UpTrendUpBar;
                else
                    results[0].BarKind = UpTrendDnBar;

                results[0].Trend = KdsTrend.Up;
            }
            else
            {
                if (Open(0) < Close(0))
                    results[0].BarKind = DnTrendUpBar;
                else
                    results[0].BarKind = DnTrendDnBar;

                results[0].Trend = KdsTrend.Down;
            }
        }

        init = true;

        if (trends[1] > 0)
        {
            if (Open(1) < Close(1))
                results[1].BarKind = UpTrendUpBar;
            else
                results[1].BarKind = UpTrendDnBar;

            results[1].Trend = KdsTrend.Up;
        }
        else if (trends[1] < 0)
        {
            if (Open(1) < Close(1))
                results[1].BarKind = DnTrendUpBar;
            else
                results[1].BarKind = DnTrendDnBar;

            results[1].Trend = KdsTrend.Down;
        }

        UpdateTrueRangeDouble(Max(Max(High(1), Close(2)), 
            High(0)) - Min(Min(Low(1), Close(2)), Low(0)));

        if (fastMa[0].Result > slowMa[0].Result)
            preliminaryTrends[0] = 1.0;
        else if (fastMa[0].Result < slowMa[0].Result)
            preliminaryTrends[0] = -1.0;
        else
            preliminaryTrends[0] = preliminaryTrends[1];

        trends[0] = preliminaryTrends[1];

        if (candle.CandleId > BARS_TO_PRIME)
        {
            if (trends[0] > 0)
            {
                if (Open(0) < Close(0))
                    results[0].BarKind = UpTrendUpBar;
                else
                    results[0].BarKind = UpTrendDnBar;

                results[0].Trend = KdsTrend.Up;
            }
            else if (trends[0] < 0)
            {
                if (Open(0) < Close(0))
                    results[0].BarKind = DnTrendUpBar;
                else
                    results[0].BarKind = DnTrendDnBar;

                results[0].Trend = KdsTrend.Down;
            }
        }

        return results[0];
    }

    public IEnumerator<KdsResult> GetEnumerator()
    {
        foreach (var result in results)
            yield return result;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}