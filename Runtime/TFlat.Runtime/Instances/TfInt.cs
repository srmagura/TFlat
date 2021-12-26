namespace TFlat.Runtime.Instances;

internal class TfInt : TfInstance
{
    public TfInt(int value)
    {
        Value = value;
    }

    public int Value { get; private init; }

    public override string ToString()
    {
        return Value.ToString();
    }
}
