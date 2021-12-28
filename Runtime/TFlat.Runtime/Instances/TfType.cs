namespace TFlat.Runtime.Instances;

internal class TfType : TfInstance
{
    public TfType(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public string Name { get; private init; }

    public override String ToString()
    {
        return Name;
    }
}
