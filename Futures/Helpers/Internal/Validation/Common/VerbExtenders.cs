using System.Runtime.CompilerServices;

namespace SquidEyes.Futures;

public static class VerbExtenders
{
    public static Must<T> Must<T>(this T value, Func<T, bool> canEval = null!,
        [CallerArgumentExpression(nameof(value))] string argName = null!)
    {
        return new Must<T>(value, argName, canEval ?? (v => true));
    }

    public static MayNot<T> MayNot<T>(this T value, Func<T, bool> canEval = null!,
        [CallerArgumentExpression(nameof(value))] string argName = null!)
    {
        return new MayNot<T>(value, argName, canEval ?? (v => true));
    }
}