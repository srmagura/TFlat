namespace TFlat.Runtime.Instances;

internal class TfFloat : TfInstance
{
    public TfFloat(double value)
    {
        Value = value;
    }

    public double Value { get; private init; }

    public override string ToString()
    {
        return Value.ToString();
    }

    public override TfInstance ApplyUnaryOperator(UnaryOperator unaryOperator)
    {
        return unaryOperator switch
        {
            UnaryOperator.NumericNegation => new TfFloat(-Value),

            _ => throw TfRuntimeException.OperatorCannotBeApplied(unaryOperator.ToString(), GetTfType())
        };
    }

    public override TfInstance ApplyBinaryOperator(BinaryOperator binaryOperator, TfInstance operand1)
    {
        if (operand1 is TfInt int1)
            operand1 = new TfFloat(int1.Value);

        if (operand1 is not TfFloat float1)
            throw new NotImplementedException();

        return binaryOperator switch
        {
            BinaryOperator.Exponentiation => new TfFloat(Math.Pow(Value, float1.Value)),

            BinaryOperator.Multiplication => new TfFloat(Value * float1.Value),
            BinaryOperator.Division => new TfFloat((double)Value / float1.Value),
            // PURPOSEFULLY NOT SUPPORTED: BinaryOperator.IntegerDivision
            BinaryOperator.Modulus => new TfFloat(Value % float1.Value),

            BinaryOperator.Addition => new TfFloat(Value + float1.Value),
            BinaryOperator.Subtraction => new TfFloat(Value - float1.Value),

            _ => throw TfRuntimeException.OperatorCannotBeApplied(binaryOperator.ToString(), GetTfType())
        };
    }
}
