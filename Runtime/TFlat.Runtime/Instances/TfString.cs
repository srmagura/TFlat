namespace TFlat.Runtime.Instances;

internal class TfString : TfInstance
{
    public TfString(string value)
    {
        Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    public string Value { get; private init; }

    public override string ToString()
    {
        return Value;
    }
}
