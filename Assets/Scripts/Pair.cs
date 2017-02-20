class Pair<TA, TB> {
    public TA AValue { get; private set; }
    public TB BValue { get; private set; }

    public Pair(TA a, TB b)
    {
        AValue = a;
        BValue = b;
    }

    public bool Equals(Pair<TA, TB> pair)
    {
        return pair.AValue.Equals(AValue) && pair.BValue.Equals(BValue);
    }

    public override bool Equals(object o)
    {
        return this.Equals(o as Pair<TA, TB>);
    }

    public override int GetHashCode()
    {
        return AValue.GetHashCode() ^ BValue.GetHashCode();
    }
}
