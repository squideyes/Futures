namespace SquidEyes.Futures.Feeds;

public interface IBarHandler
{
    Task HandleBarAsync(BarArgs args);
}