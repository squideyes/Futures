// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public class Market
{
    internal Market(string name)
    {
        Name = name;
        From = -TimeSpan.FromHours(6);
        Until = new TimeSpan(0, 16, 59, 59, 999);
    }

    public string Name { get; }
    public TimeSpan From { get; }
    public TimeSpan Until { get; }
}