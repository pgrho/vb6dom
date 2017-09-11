namespace Shipwreck.VB6Models.Expressions
{
    public sealed class UnaryExpression : Expression
    {
        public UnaryExpression(Expression operand, UnaryOperator @operator)
        {
            Operand = operand;
            Operator = @operator;
        }

        public Expression Operand { get; }

        public UnaryOperator Operator { get; }
    }
}