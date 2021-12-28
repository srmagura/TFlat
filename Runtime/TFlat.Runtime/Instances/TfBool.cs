namespace TFlat.Runtime.Instances;

internal class TfBool : TfInstance
{
    public TfBool(bool value)
    {
        Value = value;
    }

    public bool Value { get; private init; }

    public override string ToString()
    {
        return Value.ToString();
    }
}
