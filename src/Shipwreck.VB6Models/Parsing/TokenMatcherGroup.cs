using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Shipwreck.VB6Models.Parsing
{
    internal class TokenMatcherGroup : TokenMatcherItemBase, ITokenMatcherItemGroup<TokenMatcherGroup>
    {
        private readonly Delegate _CaptureConversion;

        private readonly List<TokenMatcherItemBase> _Items;

        public TokenMatcherGroup(string captureName, Delegate captureConversion)
            : base(captureName)
        {
            _Items = new List<TokenMatcherItemBase>();
            _CaptureConversion = captureConversion;
        }

        public TokenMatcherGroup AddItem(TokenMatcherItemBase item)
        {
            _Items.Add(item);
            return this;
        }

        internal override IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state)
        {
            var s = new TokenMatcherState(state.Tokens);
            s.Index = state.Index;

            var ens = new Stack<IEnumerator<TokenMatcherState>>();
            var indexes = new Stack<int>();

            indexes.Push(state.Index);
            ens.Push(_Items[0].EnumerateMatches(s).GetEnumerator());

            while (ens.Any())
            {
                var p = ens.Peek();

                if (p.MoveNext())
                {
                    if (ens.Count == _Items.Count)
                    {
                        state.Index = p.Current.Index;

                        foreach (var kv in p.Current.Captures)
                        {
                            state.Captures[kv.Key] = kv.Value;
                        }

                        if (CaptureName != null)
                        {
                            state.Captures[CaptureName] = _CaptureConversion == null ? p.Current : _CaptureConversion.DynamicInvoke(p.Current);
                        }

                        yield return state;
                    }
                    else
                    {
                        indexes.Push(p.Current.Index);
                        ens.Push(_Items[ens.Count].EnumerateMatches(p.Current).GetEnumerator());
                    }
                }
                else
                {
                    s.Index = indexes.Pop();
                    ens.Pop();
                }
            }
        }

        public override void WriteTo(TextWriter writer)
        {
            writer.Write('(');
            foreach (var item in _Items)
            {
                item.WriteTo(writer);
            }
            writer.Write(')');
        }
    }
}