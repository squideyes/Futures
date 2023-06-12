// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures.Models;

public interface IBar
{
    int BarId { get; }
    Asset Asset { get; }
    DateTime OpenOn { get; }
    float Open { get; }
    BarSpec BarSpec { get; }

    float Close { get; set; }
    DateTime CloseOn { get; set; }
    float High { get; set; }
    float Low { get; set; }
    int Volume { get; set; }

    TimeSpan Elapsed { get; }

    string ToString();
    string ToString(DateTimeFormat dateTimeFormat, BarPrefix barTimeFormat);
}