namespace Shipwreck.VB6Models.Expressions
{
    public sealed class ConstantExpression : Expression
    {
        public ConstantExpression(object value)
        {
            Value = value;
        }

        public object Value { get; }
    }
}