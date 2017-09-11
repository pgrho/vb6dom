using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenMatcher<T>
    {
        private readonly TokenMatcherBuilder _Matcher;
        private readonly Action<TokenMatcherState, T> _Callback;

        public TokenMatcher(TokenMatcherBuilder matcher, Action<TokenMatcherState, T> callback)
        {
            _Matcher = matcher;
            _Callback = callback;
        }

        public bool TryMatch(IReadOnlyList<Token> tokens, T state)
        {
            foreach (var s in _Matcher.EnumerateMatches(tokens, 0))
            {
                if (s.Index == tokens.Count)
                {
                    _Callback(s, state);
                    return true;
                }
            }
            return false;
        }
    }
}