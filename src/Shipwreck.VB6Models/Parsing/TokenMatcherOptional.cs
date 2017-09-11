using System.Collections.Generic;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenMatcherOptional : TokenMatcherItemBase
    {
        private readonly TokenMatcherItemBase _InternalItem;

        public TokenMatcherOptional(TokenMatcherItemBase internalItem)
            : base(internalItem.CaptureName)
        {
            _InternalItem = internalItem;
        }

        internal override IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state)
        {
            var i = state.Index;
            foreach (var s in _InternalItem.EnumerateMatches(state))
            {
                yield return s;
            }

            state.Index = i;
            if (_InternalItem.CaptureName != null)
            {
                state.Captures.Remove(_InternalItem.CaptureName);
            }
            yield return state;
        }
    }
}