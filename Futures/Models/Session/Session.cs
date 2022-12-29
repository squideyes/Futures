// ********************************************************
// The use of this source code is licensed under the terms
// of the MIT License (https://opensource.org/licenses/MIT)
// ********************************************************

using System.Text.Json.Serialization;
using static System.TimeOnly;

namespace SquidEyes.Futures;

public class Session : IEquatable<Session>
{
    private readonly List<Embargo> embargoes = new();

    private readonly int preNewsMinutes;
    private readonly int postNewsMinutes;

    private Session(Asset asset, TradeDate tradeDate,
        int preNewsMinutes, int postNewsMinutes, int eosNoInvestMinutes)
    {
        TradeDate = tradeDate;
        this.preNewsMinutes = preNewsMinutes;
        this.postNewsMinutes = postNewsMinutes;

        MinDateTime = tradeDate.AsDateTime().Add(
            asset.SessionSpan.From.ToTimeSpan());
        MinTickOn = TickOn.From(MinDateTime);
        MinTimeOnly = FromDateTime(MinDateTime);

        MaxDateTime = tradeDate.AsDateTime().Add(
            asset.SessionSpan.Until.ToTimeSpan());
        MaxTickOn = TickOn.From(MaxDateTime);
        MaxTimeOnly = FromDateTime(MaxDateTime);

        var from = MaxDateTime.Add(TimeSpan.FromMilliseconds(1))
            .AddMinutes(-eosNoInvestMinutes);

        AddEmbargo(FromDateTime(from), 
            FromDateTime(MaxDateTime), "EOS No-Invest");
    }

    public TradeDate TradeDate { get; }

    [JsonPropertyName("From")]
    public TimeOnly MinTimeOnly { get; }

    [JsonPropertyName("Until")]
    public TimeOnly MaxTimeOnly { get; }

    [JsonIgnore]
    public DateTime MinDateTime { get; }

    [JsonIgnore]
    public TickOn MinTickOn { get; }

    [JsonIgnore]
    public DateTime MaxDateTime { get; }

    [JsonIgnore]
    public TickOn MaxTickOn { get; }

    public List<Embargo> Embargoes => embargoes.Skip(1).ToList();

    public override string ToString() => TradeDate.ToString();

    public bool InSession(TickOn tickOn) =>
        tickOn.AsFunc(t => t >= MinTickOn && t < MaxTickOn);

    public bool InSession(DateTime dateTime) =>
        dateTime >= MinDateTime && dateTime < MaxDateTime;

    public void AddNewsEvent(TimeOnly anchor, string name)
    {
        anchor.MayNot().BeDefault();
        anchor.Must().BeBetween(MinTimeOnly, MaxTimeOnly);
        name.MayNot().BeNullOrWhitespace();
        preNewsMinutes.Must().BeBetween(5, 30);
        postNewsMinutes.Must().BeBetween(5, 30);

        embargoes.Add(Embargo.Create(name,
            anchor, preNewsMinutes, postNewsMinutes));
    }

    public void AddEmbargo(
        TimeOnly from, TimeOnly until, string name, bool isAdHoc = false)
    {
        from.MayNot().BeDefault();
        from.Must().BeBetween(MinTimeOnly, MaxTimeOnly);
        until.MayNot().BeDefault();
        until.Must().BeBetween(MinTimeOnly, MaxTimeOnly);
        until.Must().Be(v => v > from);
        name.MayNot().BeNullOrWhitespace();

        var kind = isAdHoc ? EmbargoKind.AdHoc : EmbargoKind.Strategy;

        embargoes.Add(Embargo.Create(kind, name, from, until));
    }

    public bool IsEmbargoed(TickOn tickOn, out Embargo embargo)
    {
        tickOn.MayNot().BeDefault();

        embargo = embargoes.FirstOrDefault(e => e.IsEmbargoed(tickOn))!;

        return embargo != null;
    }

    public static Session From(Asset asset, TradeDate tradeDate,
        int preNewsMinutes = 10, int postNewsMinutes = 15, int eosNoTradeMinutes = 30)
    {
        tradeDate.MayNot().BeDefault();
        tradeDate.Must().Be(v => Known.TradeDates.ContainsKey(v.AsDateOnly()));
        preNewsMinutes.Must().BeBetween(5, 30);
        postNewsMinutes.Must().BeBetween(5, 30);
        eosNoTradeMinutes.Must().BeBetween(15, 60);

        return new Session(asset, tradeDate,
            preNewsMinutes, postNewsMinutes, eosNoTradeMinutes);
    }

    public bool Equals(Session? other) =>
        other != null && TradeDate == other.TradeDate;

    public override bool Equals(object? other) =>
        other is Session session && Equals(session);

    public override int GetHashCode() => TradeDate.GetHashCode();

    public static bool operator ==(Session? lhs, Session? rhs)
    {
        if (lhs is null)
            return rhs is null;

        return lhs.Equals(rhs);
    }

    public static bool operator !=(Session? lhs, Session? rhs) =>
        !(lhs == rhs);
}