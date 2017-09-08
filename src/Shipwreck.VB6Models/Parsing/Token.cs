namespace Shipwreck.VB6Models.Parsing
{
    public sealed class Token
    {
        public Token(TokenType type, int line, int column, string text)
        {
            Type = type;
            Line = line;
            Column = column;
            Text = text;

            if (type == TokenType.Identifier)
            {
                switch (text.ToLowerInvariant())
                {
                    case "true":
                    case "false":
                        Type = TokenType.Boolean;
                        break;

                    case "mod":
                    case "and":
                    case "or":
                    case "xor":
                        Type = TokenType.Operator;
                        break;

                    case "begin":
                    case "end":
                    case "beginproperty":
                    case "endproperty":
                    case "option":

                    case "sub":
                    case "function":
                    case "property":
                    case "get":
                    case "set":
                    case "let":

                    case "private":
                    case "public":
                    case "dim":
                    case "const":
                    case "as":
                    case "redim":

                    case "return":
                    case "call":
                    case "goto":
                    case "on":

                    case "if":
                    case "then":
                    case "else":
                    case "select":
                    case "case":
                    case "do":
                    case "while":
                    case "until":
                    case "loop":
                    case "for":
                    case "each":
                    case "to":
                    case "step":
                    case "next":
                    case "exit":

                        Type = TokenType.Keyword;
                        break;
                }
            }
        }

        public Token(TokenType type, int line, int column, char c)
        {
            Type = type;
            Line = line;
            Column = column;
            Text = new string(c, 1);
        }

        public TokenType Type { get; }
        public int Line { get; }
        public int Column { get; }
        public string Text { get; }

        public override string ToString()
            => Text;
    }
}