// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using FluentValidation;
using SquidEyes.Fundamentals;
using SquidEyes.Futures.Models;

namespace SquidEyes.Futures.Feeds;

public class FeedArgs
{
    public class Validator : AbstractValidator<FeedArgs>
    {
        public Validator()
        {
            RuleFor(x => x.Tag)
                .NotEmpty();

            RuleFor(x => x.Asset)
                .NotNull();

            RuleFor(x => x.Session)
                .NotNull();

            RuleFor(x => x.BarSpec)
                .NotEmpty();

            RuleFor(x => x.TickSkip)
                .Must(v => v.IsTickSkip());

            RuleFor(x => x.MiscArgs)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .Must(v => !v!.IsEmpty)
                .WithMessage("" +
                "'{PropertyName}' must be NULL or non-empty.")
                .When(v => v.MiscArgs is not null);
        }
    }

    public Tag Tag { get; init; }
    public Asset? Asset { get; init; }
    public Session? Session { get; init; }
    public BarSpec BarSpec { get; init; }
    public int TickSkip { get; init; }
    public ArgSet? MiscArgs { get; init; }
}