using System.Collections.Generic;
using System.IO;
using System.Linq;
using Shipwreck.VB6Models.Expressions;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenMatcherExpressionItem : TokenMatcherItemBase
    {
        public TokenMatcherExpressionItem(string captureName)
            : base(captureName)
        {
        }

        public override void WriteTo(TextWriter writer)
        {
            if (CaptureName != null)
            {
                writer.Write("(?<");
                writer.Write(CaptureName);
                writer.Write(">)");
                writer.Write(')');
            }

            writer.Write("{EXPRESSION}");
        }

        internal override IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state)
        {
            var nest = 0;
            var last = state.Tokens.Count - 1;
            for (var i = state.Index; i <= last; i++)
            {
                var t = state.Tokens[i];

                if (t.IsOperatorOf("("))
                {
                    nest++;
                }

                if (nest == 0)
                {
                    if (t.IsOperatorOf(")")
                        || t.IsOperatorOf(",")
                        || t.IsOperatorOf(":")
                        || t.IsKeywordOf("Then")
                        || t.IsKeywordOf("To"))
                    {
                        last = i - 1;
                        break;
                    }
                }
                else
                {
                    if (t.IsOperatorOf(")"))
                    {
                        nest--;
                    }
                }
            }

            if (nest == 0
                && state.Index <= last)
            {
                if (CaptureName != null)
                {
                    if (!TryCreateExpression(state.Tokens, state.Index, last - state.Index + 1, out var e))
                    {
                        return Enumerable.Empty<TokenMatcherState>();
                    }

                    state.Captures[CaptureName] = e;
                }
                state.Index = last + 1;

                return new[] { state };
            }

            return Enumerable.Empty<TokenMatcherState>();
        }

        private static bool TryCreateExpression(IReadOnlyList<Token> tokens, int startIndex, int length, out Expression expression)
        {
            var buf = new List<object>(length);

            for (var i = 0; i < length; i++)
            {
                buf.Add(tokens[i + startIndex]);
            }

            // paren

            // primary: member, call, new, literal,identifier
            for (var i = 0; i < buf.Count; i++)
            {
                var t = buf[i] as Token;
                if (t != null)
                {
                    switch (t.Type)
                    {
                        case TokenType.Boolean:
                        case TokenType.Integer:
                        case TokenType.Float:
                        case TokenType.Date:
                        case TokenType.String:
                            buf[i] = new ConstantExpression(t.GetValue());
                            break;
                    }
                }
            }

            // ^

            // unary + -

            for (var i = 1; i < buf.Count; i++)
            {
                var ex = buf[i] as Expression
                            ?? (buf[i] is IList<Expression> el && el.Count == 1 ? el[0] : null);

                if (ex != null)
                {
                    var t = buf[i - 1] as Token;
                    if (t.IsOperatorOf("+")
                        || t.IsOperatorOf("-"))
                    {
                        var pt = i > 1 ? buf[i - 2] as Token : null;

                        if (i == 1 || pt != null)
                        {
                            if (t.IsOperatorOf("+"))
                            {
                                buf[i - 1] = ex;
                            }
                            else
                            {
                                buf[i - 1] = new UnaryExpression(ex, UnaryOperator.UnaryMinus);
                            }
                            buf.RemoveAt(i--);
                        }
                    }
                }
            }

            // * /

            // \

            // mod

            // binary + -

            // &

            // comparison

            // not

            // and

            // or

            // xor

            if (buf.Count == 1)
            {
                expression = buf[0] as Expression;
                return expression != null;
            }

            expression = null;
            return false;
        }
    }
}