namespace TFlat.Runtime.Instances;

internal class TfInt : TfInstance
{
    public TfInt(long value)
    {
        Value = value;
    }

    public long Value { get; private init; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override TfInstance ApplyUnaryOperator(UnaryOperator unaryOperator)
    {
        return unaryOperator switch
        {
            UnaryOperator.NumericNegation => new TfInt(-Value),

            _ => throw TfRuntimeException.OperatorCannotBeApplied(unaryOperator.ToString(), GetTfType())
        };
    }

    public override TfInstance ApplyBinaryOperator(BinaryOperator binaryOperator, TfInstance operand1)
    {
        if (operand1 is not TfInt int1)
        {
            if (operand1 is TfFloat)
            {
                return new TfFloat(Value).ApplyBinaryOperator(binaryOperator, operand1);
            }

            throw TfRuntimeException.OperatorCannotBeApplied(
                binaryOperator.ToString(), 
                GetTfType(), 
                operand1.GetTfType()
            );
        }

        return binaryOperator switch
        {
            BinaryOperator.Exponentiation => new TfFloat(Math.Pow(Value, int1.Value)),

            BinaryOperator.Multiplication => new TfInt(Value * int1.Value),
            BinaryOperator.Division => new TfFloat((double)Value / int1.Value),
            BinaryOperator.IntegerDivision => new TfInt(Value / int1.Value),
            BinaryOperator.Modulus => new TfInt(Value % int1.Value),

            BinaryOperator.Addition => new TfInt(Value + int1.Value),
            BinaryOperator.Subtraction => new TfInt(Value - int1.Value),

            _ => throw TfRuntimeException.OperatorCannotBeApplied(binaryOperator.ToString(), GetTfType())
        };
    }
}
