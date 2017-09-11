using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class TokenMatcherItem : TokenMatcherItemBase
    {
        private readonly TokenType _TypeMask;

        private readonly Regex _TextPattern;

        private readonly Delegate _CaptureConversion;

        public TokenMatcherItem(string captureName, Delegate captureConversion, TokenType typeMask, Regex textPattern)
            : base(captureName)
        {
            _TypeMask = typeMask;
            _TextPattern = textPattern;
            _CaptureConversion = captureConversion;
        }

        internal override IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state)
        {
            var t = state.Tokens.ElementAtOrDefault(state.Index);

            if (t != null
                && (_TypeMask == TokenType.Default || (_TypeMask & t.Type) != TokenType.Default)
                && _TextPattern?.IsMatch(t.Text) != false)
            {
                state.Index++;
                if (CaptureName != null)
                {
                    state.Captures[CaptureName] = _CaptureConversion == null ? t.Text : _CaptureConversion.DynamicInvoke(t);
                }
                yield return state;
            }
        }
    }
}