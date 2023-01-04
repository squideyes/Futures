// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Collections;

namespace SquidEyes.Futures;

public class SwingIndicator : IEnumerable<SwingResult>
{
    private readonly List<TickOn> closeOns = new();
    private readonly Series<double> highPlots = new();
    private readonly Series<double> highSeries = new();
    private readonly Series<double> lowPlots = new();
    private readonly Series<double> lowSeries = new();

    private readonly SlidingBuffer<double> lastLows;
    private readonly SlidingBuffer<double> lastHighs;
    private readonly int strength;
    private readonly int bufferSize;

    private double currentLow;
    private double currentHigh;
    private double lastLow;
    private double lastHigh;

    public SwingIndicator(int strength)
    {
        this.strength = strength.Must().BePositive();

        currentLow = 0;
        currentHigh = 0;
        lastLow = 0;
        lastHigh = 0;
        bufferSize = (2 * strength) + 1;

        lastLows = new SlidingBuffer<double>(bufferSize);
        lastHighs = new SlidingBuffer<double>(bufferSize);
    }

    public int Count { get; private set; } = 0;

    public SwingResult this[int index]
    {
        get
        {
            return new SwingResult(closeOns[index], highPlots[index],
                highSeries[index], lowPlots[index], lowSeries[index]);
        }
    }

    public void AddAndCalc(Candle candle)
    {
        candle.MayNot().BeNull();

        closeOns.Insert(0, candle.CloseOn);

        Count++;

        lowSeries.Add();
        highSeries.Add();
        lowPlots.Add();
        highPlots.Add();

        lowSeries[0] = 0;
        highSeries[0] = 0;

        lastLows.Add(candle.Low);
        lastHighs.Add(candle.High);

        if (lastHighs.Count == bufferSize)
        {
            var isSwingHigh = true;
            var high = lastHighs[strength];

            for (int i = 0; i < strength; i++)
            {
                if (lastHighs[i].ApproxCompare(high) >= 0)
                    isSwingHigh = false;
            }

            for (int i = strength + 1; i < lastHighs.Count; i++)
            {
                if (lastHighs[i].ApproxCompare(high) > 0)
                    isSwingHigh = false;
            }

            if (isSwingHigh)
            {
                lastHigh = currentHigh = high;

                for (int i = 0; i <= strength; i++)
                    highPlots[i] = currentHigh;
            }
            else if (candle.High > currentHigh
                || currentHigh.ApproxCompare(0.0) == 0)
            {
                currentHigh = 0.0;

                highPlots[0] = candle.Close;
            }
            else
            {
                highPlots[0] = currentHigh;
            }

            if (isSwingHigh)
            {
                for (int i = 0; i <= strength; i++)
                    highSeries[i] = lastHigh;
            }
            else
            {
                highSeries[0] = lastHigh;
            }
        }

        if (lastLows.Count == bufferSize)
        {
            bool isSwingLow = true;
            var low = lastLows[strength];

            for (int i = 0; i < strength; i++)
            {
                if (lastLows[i].ApproxCompare(low) <= 0)
                    isSwingLow = false;
            }

            for (int i = strength + 1; i < lastLows.Count; i++)
            {
                if (lastLows[i].ApproxCompare(low) < 0)
                    isSwingLow = false;
            }

            if (isSwingLow)
            {
                lastLow = currentLow = low;

                for (int i = 0; i <= strength; i++)
                    lowPlots[i] = currentLow;
            }
            else if (candle.Low < currentLow
                || currentLow.ApproxCompare(0.0) == 0)
            {
                currentLow = double.MaxValue;

                lowPlots[0] = candle.Close;
            }
            else
            {
                lowPlots[0] = currentLow;
            }

            if (isSwingLow)
            {
                for (int i = 0; i <= strength; i++)
                    lowSeries[i] = lastLow;
            }
            else
            {
                lowSeries[0] = lastLow;
            }
        }
    }

    public double GetSwingHigh(int swingToFind)
    {
        swingToFind.Must().BePositive();

        highSeries.Count.Must().Be(v => v >= swingToFind);

        if (swingToFind == 1)
            return highSeries[strength];

        var currentSwing = 1;

        for (int i = strength; i >= 0; i++)
        {
            if (highSeries[i] != highSeries[i + 1])
            {
                currentSwing++;

                if (swingToFind == currentSwing)
                    return highSeries[i + 1];
            }
        }

        return highSeries[1];
    }

    public double GetSwingLow(int swingToFind)
    {
        swingToFind.Must().BePositive();

        lowSeries.Count.Must().Be(v => v >= swingToFind);

        if (swingToFind == 1)
            return lowSeries[strength];

        var currentSwing = 1;

        for (var i = strength; i >= 0; i++)
        {
            if (lowSeries[i] != lowSeries[i + 1])
            {
                currentSwing++;

                if (swingToFind == currentSwing)
                    return lowSeries[i + 1];
            }
        }

        return lowSeries[1];
    }

    public IEnumerator<SwingResult> GetEnumerator()
    {
        for (int i = 0; i < Count; i++)
            yield return this[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}