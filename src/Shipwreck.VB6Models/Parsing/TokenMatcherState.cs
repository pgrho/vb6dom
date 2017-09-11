using System.Collections.Generic;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenMatcherState
    {
        public TokenMatcherState(IReadOnlyList<Token> tokens)
        {
            Tokens = tokens;
        }

        public IReadOnlyList<Token> Tokens { get; }

        public int Index { get; set; }

        private Dictionary<string, object> _Captures;

        internal Dictionary<string, object> Captures
            => _Captures ?? (_Captures = new Dictionary<string, object>());
    }
}