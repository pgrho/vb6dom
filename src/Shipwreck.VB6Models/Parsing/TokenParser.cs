using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenParser : IDisposable
    {
        private TextReader _Reader;
        private StringBuilder _Buffer;
        private List<Token> _TokenList;

        private int _LineIndex;

        public TokenParser(TextReader reader)
        {
            _Reader = reader;
            _Buffer = new StringBuilder();
            _TokenList = new List<Token>();
            _LineIndex = -1;
        }

        private ReadOnlyCollection<Token> _Tokens;

        public ReadOnlyCollection<Token> Tokens
            => _Tokens ?? (_Tokens = new ReadOnlyCollection<Token>(_TokenList));

        public void Dispose()
        {
            _Reader?.Dispose();
            _Reader = null;
            _Buffer = null;
            _TokenList = null;
        }

        public bool Read()
        {
            var l = _Reader.ReadLine();
            if (l == null)
            {
                return false;
            }
            _LineIndex++;

            _TokenList.Clear();
            _Buffer.Clear();

            var s = TokenType.Default;
            var tokenStart = 0;
            for (var j = 0; l != null && j < l.Length; j++)
            {
                var c = l[j];

                switch (s)
                {
                    case TokenType.Default:
                        switch (c)
                        {
                            case ' ':
                            case '\t':
                            case '\r':
                            case '\n':
                                break;

                            case '(':
                            case ')':
                            case '-':
                            case '+':
                            case '.':
                            case '=':
                            case '\\':
                            case '^':
                            case ':':
                            case ';':
                            case ',':
                            case '$': // For Unicode .frx reference
                                _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, c));
                                tokenStart = j + 1;
                                break;

                            case '<':
                                if (j + 1 < l.Length && (l[j + 1] == '>' || l[j + 1] == '='))
                                {
                                    _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, l.Substring(j, 2)));
                                    tokenStart = j + 1;
                                    j++;
                                }
                                else
                                {
                                    _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, c));
                                    tokenStart = j + 1;
                                }
                                break;

                            case '>':
                                if (j + 1 < l.Length && l[j + 1] == '>')
                                {
                                    _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, l.Substring(j, 2)));
                                    tokenStart = j + 1;
                                    j++;
                                }
                                else
                                {
                                    _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, c));
                                    tokenStart = j + 1;
                                }
                                break;

                            case '&':
                                if (j + 1 < l.Length && char.ToUpper(l[j + 1]) == 'H')
                                {
                                    tokenStart = j;
                                    _Buffer.Append("&H");
                                    s = TokenType.Integer;
                                    j++;
                                }
                                else
                                {
                                    _TokenList.Add(new Token(TokenType.Operator, _LineIndex, j, c));
                                    tokenStart = j + 1;
                                }
                                break;

                            case '"':
                                tokenStart = j;
                                s = TokenType.String;
                                break;

                            case '0':
                            case '1':
                            case '2':
                            case '3':
                            case '4':
                            case '5':
                            case '6':
                            case '7':
                            case '8':
                            case '9':
                                tokenStart = j;
                                s = TokenType.Integer;
                                _Buffer.Append(c);
                                break;

                            case '#':
                                tokenStart = j;
                                s = TokenType.Date;
                                break;

                            case '{':
                                tokenStart = j;
                                s = TokenType.Guid;
                                break;

                            case '\'':
                                _TokenList.Add(new Token(TokenType.Comment, _LineIndex, j, l.Substring(j)));
                                j = l.Length;
                                break;

                            case '_':
                                if (j + 1 == l.Length || l.Skip(j + 1).All(nc => char.IsWhiteSpace(nc)))
                                {
                                    // Line Continuation
                                    l = _Reader.ReadLine();
                                    _LineIndex++;
                                    j = -1;
                                    tokenStart = 0;
                                }
                                else
                                {
                                    tokenStart = j;
                                    s = TokenType.Identifier;
                                    _Buffer.Append(c);
                                }
                                break;

                            default:
                                tokenStart = j;
                                s = TokenType.Identifier;
                                _Buffer.Append(c);
                                break;
                        }
                        break;

                    case TokenType.String:
                        if (c == '"')
                        {
                            if (j + 1 < l.Length && l[j + 1] == '"')
                            {
                                _Buffer.Append('"');
                                j++;
                                break;
                            }
                            EndTokenWithoutCurrent(ref s, ref tokenStart, j);
                        }
                        else
                        {
                            _Buffer.Append(c);
                        }
                        break;

                    case TokenType.Integer:
                        if (('0' <= c && c <= '9')
                            || ('A' <= c && c <= 'F')
                            || ('a' <= c && c <= 'f'))
                        {
                            _Buffer.Append(c);
                        }
                        else if (c == '.')
                        {
                            s = TokenType.Float;
                            _Buffer.Append(c);
                        }
                        else if (c.IsTypeSuffix())
                        {
                            EndTokenWithCurrent(ref s, ref tokenStart, j, c);
                        }
                        else
                        {
                            EndTokenAndRepeatCurrent(ref s, ref tokenStart, ref j);
                        }
                        break;

                    case TokenType.Float:
                        if ('0' <= c && c <= '9')
                        {
                            _Buffer.Append(c);
                        }
                        else if (c.IsTypeSuffix())
                        {
                            EndTokenWithCurrent(ref s, ref tokenStart, j, c);
                        }
                        else
                        {
                            EndTokenAndRepeatCurrent(ref s, ref tokenStart, ref j);
                        }
                        break;

                    case TokenType.Date:
                        if (c == '#')
                        {
                            EndTokenWithoutCurrent(ref s, ref tokenStart, j);
                        }
                        else
                        {
                            _Buffer.Append(c);
                        }
                        break;

                    case TokenType.Guid:
                        if (c == '}')
                        {
                            EndTokenWithoutCurrent(ref s, ref tokenStart, j);
                        }
                        else
                        {
                            _Buffer.Append(c);
                        }
                        break;

                    case TokenType.Identifier:
                        if (('A' <= c && c <= 'Z')
                            || ('a' <= c && c <= 'z')
                            || ('0' <= c && c <= '9')
                            || c == '_')
                        {
                            _Buffer.Append(c);
                        }
                        else if (c.IsTypeSuffix())
                        {
                            EndTokenWithCurrent(ref s, ref tokenStart, j, c);
                        }
                        else
                        {
                            EndTokenAndRepeatCurrent(ref s, ref tokenStart, ref j);
                        }
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            if (_Buffer.Length > 0)
            {
                switch (s)
                {
                    case TokenType.Identifier:
                    case TokenType.Integer:
                    case TokenType.Float:
                        _TokenList.Add(new Token(s, _LineIndex, tokenStart, _Buffer.ToString()));
                        _Buffer.Clear();
                        break;

                    default:
                        throw new NotImplementedException();
                }
            }

            return true;
        }

        private void EndTokenWithCurrent(ref TokenType s, ref int tokenStart, int j, char c)
        {
            _Buffer.Append(c);
            EndTokenWithoutCurrent(ref s, ref tokenStart, j);
        }

        private void EndTokenWithoutCurrent(ref TokenType s, ref int tokenStart, int j)
        {
            _TokenList.Add(new Token(s, _LineIndex, tokenStart, _Buffer.ToString()));
            _Buffer.Clear();
            s = TokenType.Default;
            tokenStart = j + 1;
        }

        private void EndTokenAndRepeatCurrent(ref TokenType s, ref int tokenStart, ref int j)
        {
            _TokenList.Add(new Token(s, _LineIndex, tokenStart, _Buffer.ToString()));
            _Buffer.Clear();
            s = TokenType.Default;
            tokenStart = j;
            j--;
        }
    }
}