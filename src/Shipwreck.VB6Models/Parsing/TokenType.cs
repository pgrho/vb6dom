namespace Shipwreck.VB6Models.Parsing
{
    public enum TokenType
    {
        Default,
        Operator = 1 << 0,
        Identifier = 1 << 1,
        String = 1 << 2,
        Integer = 1 << 3,
        Float = 1 << 4,
        Date = 1 << 5,
        Guid = 1 << 6,
        Comment = 1 << 7,

        // promoted from identifier

        Boolean = 1 << 8,
        Keyword = 1 << 9,


        Value = Boolean | Integer | Float | Date | String
    }
}