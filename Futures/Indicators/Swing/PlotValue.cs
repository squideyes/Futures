// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public struct PlotValue
{
    public PlotValue(double value, bool isValid = true)
    {
        Value = value;
        IsValid = isValid;
    }

    public double Value { get; }

    public bool IsValid { get; private set; }

    public void Reset() => IsValid = false;
}