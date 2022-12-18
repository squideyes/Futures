namespace SquidEyes.Futures;

public class VerbException : ArgumentException
{
    public VerbException(string argame, string message)
        : base($"{message} (Argument: \"{argame}\")")
    {
    }
}