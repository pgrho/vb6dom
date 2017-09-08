using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Shipwreck.VB6Models.Forms;

namespace Shipwreck.VB6Models.Parsing
{
    public sealed class SourceFileReader : ISourceReadingState
    {
        private Stack<ISourceReadingState> _States;

        public SourceFileReader(string fileName)
        {
            FileName = Path.GetFullPath(fileName);
        }

        public string FileName { get; }

        public string Version { get; private set; }

        private List<FormObject> _FormObjects;

        public List<FormObject> FormObjects
            => _FormObjects ?? (_FormObjects = new List<FormObject>());

        public void Load()
        {
            _States = new Stack<ISourceReadingState>();
            _States.Push(this);

            using (var sr = new StreamReader(FileName))
            using (var tp = new TokenParser(sr))
            {
                while (_States.Any() && tp.Read())
                {
                    if (!tp.Tokens.Any())
                    {
                        continue;
                    }

                    if (!_States.Peek().Accept(this, tp.Tokens))
                    {
                        _States.Pop();
                    }
                }
            }

            _States = null;
        }

        internal void Push(ISourceReadingState state)
            => _States.Push(state);

        internal void OnUnknownTokens(ISourceReadingState state, IReadOnlyList<Token> tokens)
            => Console.WriteLine("Encountered unknown token sequence in {0}: {1}", state, string.Concat(tokens.Select(t => $"{t.Text}({t.Type})")));

        bool ISourceReadingState.Accept(SourceFileReader reader, IReadOnlyList<Token> tokens)
        {
            var ft = tokens.First();

            switch (ft.Type)
            {
                case TokenType.Identifier:
                    if ("VERSION".EqualsIgnoreCase(ft.Text))
                    {
                        if (tokens.Count == 2)
                        {
                            Version = tokens[1].Text;
                            return true;
                        }
                    }
                    else if ("Attribute".EqualsIgnoreCase(ft.Text))
                    {
                    }
                    break;

                case TokenType.Keyword:
                    if (FormReadingState.TryCreateControl(tokens, out var c))
                    {
                        FormObjects.Add(c);
                        Push(new FormReadingState(c));
                        return true;
                    }
                    else if (tokens.Any(t => t.Type == TokenType.Keyword && t.Text.EqualsIgnoreCase("Declare")))
                    {
                    }
                    else if (tokens.Any(t => t.Type == TokenType.Keyword && t.Text.EqualsIgnoreCase("Sub", "Function", "Property")))
                    {
                    }

                    break;
            }

            OnUnknownTokens(this, tokens);

            return true;
        }
    }
}