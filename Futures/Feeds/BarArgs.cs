// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public class BarArgs : EventArgs
{
    public BarArgs(Bar bar, Tick tick, Tag tag, Asset asset)
    {
        Bar = bar.MayNot().BeNull();
        Tick = tick.MayNot().BeDefault();
        Tag = tag.MayNot().BeDefault();
        Asset = asset.MayNot().BeDefault();
    }

    public Bar Bar { get; }
    public Tick Tick { get; }
    public Tag Tag { get; }
    public Asset Asset { get; }
}