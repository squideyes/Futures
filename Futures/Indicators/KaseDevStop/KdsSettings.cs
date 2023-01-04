// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using SquidEyes.Futures;

namespace SquidEyes.Futures;

public class KdsSettings : IValidatable
{
    //internal class Validator : AbstractValidator<KdsSettings>
    //{
    //    public Validator()
    //    {
    //        RuleFor(x => x.AnchorMode).IsInEnum();
    //        RuleFor(x => x.FastMA).IsInEnum();
    //        RuleFor(x => x.SlowMA).IsInEnum();
    //        RuleFor(x => x.FastPeriod).InclusiveBetween(5,99);
    //        RuleFor(x => x.SlowPeriod).InclusiveBetween(5, 99);
    //        RuleFor(x => x.AvgTbtrPeriod).InclusiveBetween(5, 99);
    //        RuleFor(x => x.AvgTbtrMultiplier).GreaterThan(0.0);
    //        RuleFor(x => x.StdDevPeriod).InclusiveBetween(5, 99);
    //        RuleFor(x => x.StdDevMultiplier1).GreaterThan(0.0);
    //        RuleFor(x => x.StdDevMultiplier2).GreaterThan(0.0);
    //        RuleFor(x => x.StdDevMultiplier3).GreaterThan(0.0);
    //    }
    //}

    public required KdsAnchorMode AnchorMode { get; init; } = KdsAnchorMode.HighLow;
    public required bool TrailingStop { get; init; } = true;
    public required MaKind FastMA { get; init; } = MaKind.SMA;
    public required MaKind SlowMA { get; init; } = MaKind.SMA;
    public required int FastPeriod { get; init; } = 14;
    public required int SlowPeriod { get; init; } = 21;
    public required int AvgTbtrPeriod { get; init; } = 30;
    public required double AvgTbtrMultiplier { get; init; } = 1.0;
    public required int StdDevPeriod { get; init; } = 30;
    public required double StdDevMultiplier1 { get; init; } = 1.0;
    public required double StdDevMultiplier2 { get; init; } = 2.6;
    public required double StdDevMultiplier3 { get; init; } = 3.3;

    public void Validate()
    {
    }
}