using System.Collections.Generic;
using System.IO;

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

        public abstract void WriteTo(TextWriter writer);

        public override string ToString()
        {
            using (var sw = new StringWriter())
            {
                WriteTo(sw);
                return sw.ToString();
            }
        }
    }
}