using System.Collections.Generic;

namespace Shipwreck.VB6Models.Parsing
{
    internal abstract class TokenMatcherItemBase
    {
        protected TokenMatcherItemBase(string captureName)
        {
            CaptureName = captureName;
        }

        public string CaptureName { get; }

        internal abstract IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state);
    }
}