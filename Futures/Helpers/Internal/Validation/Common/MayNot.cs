namespace SquidEyes.Futures;

public class MayNot<T> : VerbBase<T, MayNot<T>>
{
    public MayNot(T value, string argName, Func<T, bool> canEval)
        : base(value, argName, canEval, "Value may not")
    {
    }
}