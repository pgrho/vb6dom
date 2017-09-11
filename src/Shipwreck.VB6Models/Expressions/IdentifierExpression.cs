namespace Shipwreck.VB6Models.Expressions
{
    public sealed class IdentifierExpression : Expression
    {
        public IdentifierExpression(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; }
    }
}