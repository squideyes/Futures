// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.FeedViewer;

public static class ChartHelper
{
    public static List<ChartData> GetChartDatas(
        this TickSet tickSet, int brickTicks)
    {
        var feed = TdRenkoFeed.Create(
            tickSet.Contract.Asset, BrickTicks.From(brickTicks));

        var datas = new List<ChartData>();

        feed.OnCandle += (s, e) =>
        {
            datas.Add(new ChartData()
            {
                CloseOn = e.Candle.CloseOn.AsDateTime(),
                Open = e.Candle.Open,
                High = e.Candle.High,
                Low = e.Candle.Low,
                Close = e.Candle.Close
            });
        };

        for (var i = 0; i < tickSet.Count; i++)
            feed.HandleTick(tickSet[i], i == tickSet.Count - 1);

        return datas;
    }
}