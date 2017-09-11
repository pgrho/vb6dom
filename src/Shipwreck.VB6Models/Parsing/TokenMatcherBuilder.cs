using System;
using System.Collections.Generic;
using System.Linq;

namespace Shipwreck.VB6Models.Parsing
{
    internal class TokenMatcherBuilder : ITokenMatcherItemGroup<TokenMatcherBuilder>
    {
        private readonly List<TokenMatcherItemBase> _Items = new List<TokenMatcherItemBase>();

        public TokenMatcherBuilder AddItem(TokenMatcherItemBase item)
        {
            _Items.Add(item);
            return this;
        }

        internal IEnumerable<TokenMatcherState> EnumerateMatches(IReadOnlyList<Token> tokens, int index)
        {
            var s = new TokenMatcherState(tokens) { Index = index };

            var ens = new Stack<IEnumerator<TokenMatcherState>>();
            var indexes = new Stack<int>();

            indexes.Push(s.Index);
            ens.Push(_Items[0].EnumerateMatches(s).GetEnumerator());

            while (ens.Any())
            {
                var p = ens.Peek();

                if (p.MoveNext())
                {
                    if (ens.Count == _Items.Count)
                    {
                        s.Index = p.Current.Index;

                        yield return s;
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

        public TokenMatcher<T> ToMatcher<T>(Action<TokenMatcherState, T> callback)
            => new TokenMatcher<T>(this, callback);
    }
}