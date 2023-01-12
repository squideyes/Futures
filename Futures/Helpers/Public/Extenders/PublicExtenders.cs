namespace SquidEyes.Futures;

public static class PublicExtenders
{
    public static string ToCode(this Source value)
    {
        return value switch
        {
            Source.Kibot => "KB",
            Source.Amp => "AMP",
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }

    public static Source ToSource(this string value)
    {
        return value switch
        {
            "KB" => Source.Kibot,
            "AMP" => Source.Amp,
            _ => throw new ArgumentOutOfRangeException(nameof(value))
        };
    }
}
