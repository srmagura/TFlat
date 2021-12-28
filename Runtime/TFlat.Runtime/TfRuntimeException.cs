using TFlat.Runtime.Instances;

namespace TFlat.Runtime;

public class TfRuntimeException : Exception
{
    public TfRuntimeException(string? message) : base(message)
    {
    }

    public TfRuntimeException(string? message, Exception? innerException) 
        : base(message, innerException)
    {
    }

    internal static TfRuntimeException OperatorCannotBeApplied(string @operator, TfType operandType) =>
        throw new TfRuntimeException($"The operator {@operator} cannot be applied to {operandType.Name}.");

    internal static TfRuntimeException OperatorCannotBeApplied(string @operator, TfType operand0Type, TfType operand1Type) =>
        throw new TfRuntimeException($"The operator {@operator} cannot be applied to {operand0Type} and {operand1Type}.");
}
