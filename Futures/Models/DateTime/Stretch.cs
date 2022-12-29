// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

namespace SquidEyes.Futures;

public readonly struct Stretch<T> 
    where T : IComparable<T>
{
    internal Stretch(T from, T until)
    {
        From = from;
        Until = until;
    }

    public T From { get; }
    public T Until { get; }
}