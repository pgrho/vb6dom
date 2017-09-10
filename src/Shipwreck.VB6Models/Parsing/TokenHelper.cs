using System;
using Shipwreck.VB6Models.Declarations;

namespace Shipwreck.VB6Models.Parsing
{
    internal static class TokenHelper
    {
        public static bool IsKeywordOf(this Token token, string keyword)
            => token.Match(TokenType.Keyword, keyword);

        public static bool IsOperatorOf(this Token token, string @operator)
            => token.Match(TokenType.Operator, @operator);

        public static bool Match(this Token token, TokenType type, string text)
            => token != null && (token.Type & type) != TokenType.Default && token.Text.EqualsIgnoreCase(text);

        internal static bool IsTypeSuffix(this char c)
            => c == '%' || c == '&' || c == '$' || c == '!' || c == '#' || c == '@';

        internal static ITypeReference TypeFromSuffix(this char c)
        {
            switch (c)
            {
                case '%':
                    return VB6Types.Integer;

                case '&':
                    return VB6Types.Long;

                case '$':
                    return VB6Types.String;

                case '!':
                    return VB6Types.Single;

                case '#':
                    return VB6Types.Double;

                case '@':
                    return VB6Types.Currency;
            }

            throw new Exception();
        }

        internal static ITypeReference TypeFromName(this string type)
        {
            if (type.EqualsIgnoreCase("Boolean"))
            {
                return VB6Types.Boolean;
            }
            if (type.EqualsIgnoreCase("Integer"))
            {
                return VB6Types.Integer;
            }
            if (type.EqualsIgnoreCase("Long"))
            {
                return VB6Types.Long;
            }
            if (type.EqualsIgnoreCase("Single"))
            {
                return VB6Types.Single;
            }
            if (type.EqualsIgnoreCase("Double"))
            {
                return VB6Types.Double;
            }
            if (type.EqualsIgnoreCase("Currency"))
            {
                return VB6Types.Currency;
            }
            if (type.EqualsIgnoreCase("String"))
            {
                return VB6Types.String;
            }
            //if (type.EqualsIgnoreCase("Date"))
            //{
            //    return VB6Types.Date;
            //}
            throw new Exception();
        }

        public static object Negate(this object v)
        {
            if (v is int iv)
            {
                return -iv;
            }
            else if (v is long lv)
            {
                return -lv;
            }
            else if (v is float sv)
            {
                return -sv;
            }
            else if (v is double dv)
            {
                return -dv;
            }
            else
            {
                return -((IConvertible)v).ToDecimal(null);
            }
        }
    }
}