namespace Shipwreck.VB6Models.Parsing
{
    internal static class TokenFactory
    {
        public static Token ID(string text)
            => TOKEN(TokenType.Identifier, text);

        public static Token KW(string text)
            => TOKEN(TokenType.Keyword, text);

        public static Token OP(string text)
            => TOKEN(TokenType.Operator, text);

        public static Token ST(string text)
            => TOKEN(TokenType.String, text);

        public static Token IN(string text)
            => TOKEN(TokenType.Integer, text);

        public static Token FL(string text)
            => TOKEN(TokenType.Float, text);

        public static Token BL(string text)
            => TOKEN(TokenType.Boolean, text);

        public static Token UU(string text)
            => TOKEN(TokenType.Guid, text);

        public static Token CM(string text)
            => TOKEN(TokenType.Comment, text);

        public static Token TOKEN(TokenType type, string text)
            => new Token(type, -1, -1, text);
    }
}