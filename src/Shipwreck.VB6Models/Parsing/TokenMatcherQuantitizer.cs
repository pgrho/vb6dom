using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.VB6Models.Parsing
{
    internal class TokenMatcherQuantitizer : TokenMatcherItemBase
    {
        private readonly TokenMatcherItemBase _InternalItem;
        private readonly int _Minimum;
        private readonly int _Maximum;

        public TokenMatcherQuantitizer(TokenMatcherItemBase internalItem, int minimum, int maximum)
            : base(internalItem.CaptureName)
        {
            _InternalItem = internalItem;
            _Minimum = minimum;
            _Maximum = maximum;
        }

        internal override IEnumerable<TokenMatcherState> EnumerateMatches(TokenMatcherState state)
        {
            var ens = new Stack<IEnumerator<TokenMatcherState>>();
            var states = new Stack<TokenMatcherState>();
            var captures = new List<object>();

            states.Push(state);
            ens.Push(_InternalItem.EnumerateMatches(new TokenMatcherState(state.Tokens)
            {
                Index = state.Index
            }).GetEnumerator());

            while (ens.Any())
            {
                var p = ens.Peek();

                if (p.MoveNext())
                {
                    var cs = p.Current;

                    var cap = _InternalItem.CaptureName != null && cs.Captures.TryGetValue(_InternalItem.CaptureName, out var obj) ? obj : null;
                    captures.Add(cap);

                    if (captures.Count >= _Maximum)
                    {
                        state.Index = cs.Index;

                        if (CaptureName != null)
                        {
                            state.Captures[CaptureName] = captures;
                        }

                        yield return state;

                        captures.RemoveAt(captures.Count - 1);
                    }
                    else
                    {
                        states.Push(cs);
                        ens.Push(_InternalItem.EnumerateMatches(new TokenMatcherState(state.Tokens)
                        {
                            Index = cs.Index
                        }).GetEnumerator());
                    }
                }
                else
                {
                    var cs = states.Pop();
                    ens.Pop();

                    if (_Minimum <= captures.Count && captures.Count <= _Maximum)
                    {
                        state.Index = cs.Index;

                        if (CaptureName != null)
                        {
                            state.Captures[CaptureName] = captures;
                        }

                        yield return state;

                        if (captures.Any())
                        {
                            captures.RemoveAt(captures.Count - 1);
                        }
                    }
                }
            }
        }
    }
}