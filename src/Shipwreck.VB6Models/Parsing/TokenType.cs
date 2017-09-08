namespace Shipwreck.VB6Models.Parsing
{
    public enum TokenType
    {
        Default,
        Operator,
        Identifier,
        String,
        Integer,
        Float,
        Date,
        Guid,
        Comment,

        // promoted from identifier

        Boolean,
        Keyword
    }
}