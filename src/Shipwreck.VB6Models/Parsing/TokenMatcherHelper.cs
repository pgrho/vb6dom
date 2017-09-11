using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shipwreck.VB6Models.Parsing
{
    internal static class TokenMatcherHelper
    {
        #region ContinueWith

        public static TGroup ContinueWith<TGroup>(this ITokenMatcherItemGroup<TGroup> group, TokenType typeMask, Regex textPattern)
            => group.ContinueWith(null, typeMask, textPattern);

        public static TGroup ContinueWith<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, TokenType typeMask, Regex textPattern)
            => group.AddItem(new TokenMatcherItem(captureName, null, typeMask, textPattern));

        public static TGroup ContinueWith<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, TokenType typeMask, Regex textPattern)
        => group.AddItem(new TokenMatcherItem(captureName, captureConversion, typeMask, textPattern));

        #region With TokenType

        #region Keyword

        public static TGroup ContinueWithKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string keyword)
            => group.ContinueWith(null, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, params string[] keywords)
            => group.ContinueWith(null, TokenType.Keyword, keywords.ToRegex());

        public static TGroup ContinueWithKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string keyword)
            => group.ContinueWith(captureName, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithKeyword<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, string keyword)
            => group.ContinueWith(captureName, captureConversion, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string[] keywords)
            => group.ContinueWith(captureName, TokenType.Keyword, keywords.ToRegex());

        public static TGroup ContinueWithKeyword<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, params string[] keywords)
            => group.ContinueWith(captureName, captureConversion, TokenType.Keyword, keywords.ToRegex());

        #endregion Keyword

        #region Operator

        public static TGroup ContinueWithOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string @operator)
            => group.ContinueWith(null, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, params string[] operators)
            => group.ContinueWith(null, TokenType.Operator, operators.ToRegex());

        public static TGroup ContinueWithOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string @operator)
            => group.ContinueWith(captureName, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOperator<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, string @operator)
            => group.ContinueWith(captureName, captureConversion, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string[] operators)
            => group.ContinueWith(captureName, TokenType.Operator, operators.ToRegex());

        public static TGroup ContinueWithOperator<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, params string[] operators)
            => group.ContinueWith(captureName, captureConversion, TokenType.Operator, operators.ToRegex());

        #endregion Operator

        public static TGroup ContinueWithIdentifier<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName)
            => group.ContinueWith(captureName, TokenType.Identifier, null);

        public static TGroup ContinueWithValue<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName)
            => group.ContinueWith(captureName, t => t.GetValue(), TokenType.Value, null);

        public static TGroup ContinueWithExpression<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName)
            => group.AddItem(new TokenMatcherExpressionItem(captureName));

        #endregion With TokenType

        #endregion ContinueWith

        #region ContinueWithOptional

        public static TGroup ContinueWithOptional<TGroup>(this ITokenMatcherItemGroup<TGroup> group, TokenType typeMask, Regex textPattern)
            => group.ContinueWithOptional(null, typeMask, textPattern);

        public static TGroup ContinueWithOptional<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, TokenType typeMask, Regex textPattern)
            => group.AddItem(new TokenMatcherOptional(new TokenMatcherItem(captureName, null, typeMask, textPattern)));

        public static TGroup ContinueWithOptional<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, TokenType typeMask, Regex textPattern)
            => group.AddItem(new TokenMatcherOptional(new TokenMatcherItem(captureName, captureConversion, typeMask, textPattern)));

        #region With TokenType

        #region Keyword

        public static TGroup ContinueWithOptionalKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string keyword)
            => group.ContinueWithOptional(null, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithOptionalKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, params string[] keywords)
            => group.ContinueWithOptional(null, TokenType.Keyword, keywords.ToRegex());

        public static TGroup ContinueWithOptionalKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string keyword)
            => group.ContinueWithOptional(captureName, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithOptionalKeyword<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, string keyword)
            => group.ContinueWithOptional(captureName, captureConversion, TokenType.Keyword, keyword.ToRegex());

        public static TGroup ContinueWithOptionalKeyword<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string[] keywords)
            => group.ContinueWithOptional(captureName, TokenType.Keyword, keywords.ToRegex());

        public static TGroup ContinueWithOptionalKeyword<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, params string[] keywords)
            => group.ContinueWithOptional(captureName, captureConversion, TokenType.Keyword, keywords.ToRegex());

        #endregion Keyword

        #region Operator

        public static TGroup ContinueWithOptionalOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string @operator)
            => group.ContinueWithOptional(null, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOptionalOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, params string[] operators)
            => group.ContinueWithOptional(null, TokenType.Operator, operators.ToRegex());

        public static TGroup ContinueWithOptionalOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string @operator)
            => group.ContinueWithOptional(captureName, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOptionalOperator<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, string @operator)
            => group.ContinueWithOptional(captureName, captureConversion, TokenType.Operator, @operator.ToRegex());

        public static TGroup ContinueWithOptionalOperator<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, string[] operators)
            => group.ContinueWithOptional(captureName, TokenType.Operator, operators.ToRegex());

        public static TGroup ContinueWithOptionalOperator<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Func<Token, T> captureConversion, params string[] operators)
            => group.ContinueWithOptional(captureName, captureConversion, TokenType.Operator, operators.ToRegex());

        #endregion Operator

        public static TGroup ContinueWithOptionalIdentifier<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName)
            => group.ContinueWithOptional(captureName, TokenType.Identifier, null);

        #endregion With TokenType

        #endregion ContinueWithOptional

        #region ContinueWithGroup

        public static TGroup ContinueWithGroup<TGroup>(this ITokenMatcherItemGroup<TGroup> group, Action<TokenMatcherGroup> configuration)
            => group.ContinueWithGroup(null, configuration);

        public static TGroup ContinueWithGroup<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration)
        {
            var g = new TokenMatcherGroup(captureName, null);
            configuration(g);

            return group.AddItem(g);
        }

        public static TGroup ContinueWithGroup<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration, Func<TokenMatcherState, T> captureConversion)
        {
            var g = new TokenMatcherGroup(captureName, captureConversion);
            configuration(g);

            return group.AddItem(g);
        }

        #endregion ContinueWithGroup

        #region ContinueWithOptionalGroup

        public static TGroup ContinueWithOptionalGroup<TGroup>(this ITokenMatcherItemGroup<TGroup> group, Action<TokenMatcherGroup> configuration)
            => group.ContinueWithOptionalGroup(null, configuration);

        public static TGroup ContinueWithOptionalGroup<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration)
        {
            var g = new TokenMatcherGroup(captureName, null);
            configuration(g);

            return group.AddItem(new TokenMatcherOptional(g));
        }

        public static TGroup ContinueWithOptionalGroup<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration, Func<TokenMatcherState, T> captureConversion)
        {
            var g = new TokenMatcherGroup(captureName, captureConversion);
            configuration(g);

            return group.AddItem(new TokenMatcherOptional(g));
        }

        #endregion ContinueWithOptionalGroup

        #region ContinueWithMany

        public static TGroup ContinueWithMany<TGroup>(this ITokenMatcherItemGroup<TGroup> group, Action<TokenMatcherGroup> configuration, int minimum = 1, int maximum = int.MaxValue)
            => group.ContinueWithMany(null, configuration, minimum, maximum);

        public static TGroup ContinueWithMany<TGroup>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration, int minimum = 1, int maximum = int.MaxValue)
        {
            var g = new TokenMatcherGroup(captureName, null);
            configuration(g);

            return group.AddItem(new TokenMatcherQuantitizer(g, minimum, maximum));
        }

        public static TGroup ContinueWithMany<TGroup, T>(this ITokenMatcherItemGroup<TGroup> group, string captureName, Action<TokenMatcherGroup> configuration, Func<TokenMatcherState, T> captureConversion, int minimum = 1, int maximum = int.MaxValue)
        {
            var g = new TokenMatcherGroup(captureName, captureConversion);
            configuration(g);

            return group.AddItem(new TokenMatcherQuantitizer(g, minimum, maximum));
        }

        public static TGroup ContinueWithMany<TGroup, T>(
            this ITokenMatcherItemGroup<TGroup> group,
            string captureName,
            Action<TokenMatcherGroup> configuration,
            string separator,
            Func<TokenMatcherState, T> captureConversion,
            int minimum = 1,
            int maximum = int.MaxValue)
            where T : class
            => group.ContinueWithMany(
                        captureName,
                        configuration,
                        new TokenMatcherItem(null, null, TokenType.Operator, new Regex("^" + Regex.Escape(separator) + "$")),
                        captureConversion,
                        minimum,
                        maximum);

        public static TGroup ContinueWithMany<TGroup, T>(
            this ITokenMatcherItemGroup<TGroup> group,
            string captureName,
            Action<TokenMatcherGroup> configuration,
            TokenMatcherItemBase separator,
            Func<TokenMatcherState, T> captureConversion,
            int minimum = 1,
            int maximum = int.MaxValue)
            where T : class
        {
            var firstKey = captureName + "." + nameof(TokenMatcherHelper) + "." + nameof(ContinueWithMany) + ".firstCapture";

            var fg = new TokenMatcherGroup(firstKey, captureConversion);
            configuration(fg);

            var ig = new TokenMatcherGroup(captureName, captureConversion);
            ig.AddItem(separator);
            configuration(ig);

            var igq = new TokenMatcherQuantitizer(ig, Math.Max(0, minimum - 1), maximum < int.MaxValue ? maximum - 1 : int.MaxValue);

            var g = new TokenMatcherGroup(captureName, (Func<TokenMatcherState, List<T>>)(s =>
            {
                var f = s.Captures.TryGetValue(firstKey, out var fv) ? fv as T : null;
                var l = s.Captures.TryGetValue(captureName, out var lv) ? lv as IEnumerable : null;

                if (f == null)
                {
                    return l.Cast<T>().ToList();
                }

                if (l != null)
                {
                    var nl = new List<T>() { f };
                    nl.AddRange(l.Cast<T>());
                    return nl;
                }
                else
                {
                    return new List<T>(1) { f };
                }
            }));

            g.AddItem(fg).AddItem(igq);

            return group.AddItem(g);
        }

        #endregion ContinueWithMany

        private static Regex ToRegex(this string keyword)
            => new Regex("^" + Regex.Escape(keyword) + "$", RegexOptions.IgnoreCase);

        private static Regex ToRegex(this string[] keywords)
            => new Regex("^(" + string.Join("|", keywords.Select(Regex.Escape)) + ")$", RegexOptions.IgnoreCase);
    }
}