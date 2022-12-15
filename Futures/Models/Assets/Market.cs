namespace SquidEyes.Futures;

public class Market
{
    public required string Name { get; init; }
    public required Period<TimeSpan> Period { get; init;  }
}
