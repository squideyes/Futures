namespace SquidEyes.Futures;

public class Must<T> : VerbBase<T, Must<T>>
{
    public Must(T value, string argName, Func<T, bool> canEval)
        : base(value, argName, canEval, "Value must")
    {
    }
}