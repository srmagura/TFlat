namespace TFlat.Runtime.Instances;

internal abstract class TfInstance
{
    public override abstract string ToString();

    public TfType GetTfType()
    {
        // TODO implement for real in derived classes
        return new TfType(GetType().Name);
    }

    public virtual TfInstance ApplyUnaryOperator(UnaryOperator unaryOperator)
    {
        throw TfRuntimeException.OperatorCannotBeApplied(unaryOperator.ToString(), GetTfType());
    }

    public virtual TfInstance ApplyBinaryOperator(BinaryOperator binaryOperator, TfInstance operand1)
    {
        throw TfRuntimeException.OperatorCannotBeApplied(binaryOperator.ToString(), GetTfType(), operand1.GetTfType());
    }
}
