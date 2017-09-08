using System;
using System.Globalization;

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
                    case "declare":

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

                    case "nothing":

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

        public object GetValue()
        {
            switch (Type)
            {
                case TokenType.Keyword:
                    if (Text.EqualsIgnoreCase("Nothing"))
                    {
                        return null;
                    }
                    break;

                case TokenType.Boolean:
                    return Text.EqualsIgnoreCase("True");

                case TokenType.Integer:
                    {
                        var suffix = TokenParser.IsTypeSuffix(Text.Last()) ? Text.Last() : '\0';
                        var li = suffix == '\0' ? Text.Length : (Text.Length - 1);

                        int si;
                        NumberStyles st;

                        if (Text[0] == '&')
                        {
                            si = 2;
                            st = NumberStyles.HexNumber;
                        }
                        else
                        {
                            si = 0;
                            st = NumberStyles.Integer;
                        }

                        if (long.TryParse(Text.Substring(si, li - si), st, null, out var l))
                        {
                            return ConvertBySuffix(l, suffix, typeof(short));
                        }
                        break;
                    }

                case TokenType.Float:
                    {
                        var suffix = TokenParser.IsTypeSuffix(Text.Last()) ? Text.Last() : '\0';
                        var li = suffix == '\0' ? Text.Length : (Text.Length - 1);

                        if (double.TryParse(Text.Substring(0, li), out var l))
                        {
                            return ConvertBySuffix(l, suffix, typeof(float));
                        }
                        break;
                    }

                case TokenType.Guid:
                    if (Guid.TryParse(Text, out var guid))
                    {
                        return guid;
                    }
                    break;
            }
            return Text;
        }

        private static object ConvertBySuffix(IConvertible l, char suffix, Type defaultType)
            => l.ToType(
                    suffix == '%' ? typeof(int)
                    : suffix == '&' ? typeof(int)
                    : suffix == '!' ? typeof(float)
                    : suffix == '#' ? typeof(double)
                    : suffix == '@' ? typeof(decimal)
                    : suffix == '$' ? typeof(string)
                    : defaultType, null);
    }
}