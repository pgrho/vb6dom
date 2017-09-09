using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Shipwreck.VB6Models.Declarations;

namespace Shipwreck.VB6Models.Parsing
{
    public sealed class SourceFileReader
    {
        private Stack<ISourceReadingState> _States;

        public SourceFileReader(string fileName)
        {
            FileName = Path.GetFullPath(fileName);
        }

        public string FileName { get; }

        internal Encoding Encoding { get; private set; }

        public ModuleBase Load()
        {
            ModuleBase m;
            switch (Path.GetExtension(FileName).ToLowerInvariant())
            {
                case ".frm":
                    m = new FormModule();
                    break;

                case ".cls":
                    m = new ClassModule();
                    break;

                case ".bas":
                    m = new StandardModule();
                    break;

                default:
                    throw new NotSupportedException();
            }

            _States = new Stack<ISourceReadingState>();
            _States.Push(new ModuleReadingState(m));

            using (var sr = new StreamReader(FileName))
            using (var tp = new TokenParser(sr))
            {
                Encoding = sr.CurrentEncoding;

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

            return m;
        }

        internal byte[] ReadBinary(string frxName, int offset)
        {
            using (var fs = new FileStream(new Uri(new Uri(FileName), frxName).LocalPath, FileMode.Open))
            using (var br = new BinaryReader(fs))
            {
                var length = br.ReadInt32();
                return br.ReadBytes(length);
            }
        }

        internal void Push(ISourceReadingState state)
            => _States.Push(state);

        internal void OnUnknownTokens(ISourceReadingState state, IReadOnlyList<Token> tokens)
            => Console.WriteLine("Encountered unknown token sequence in {0}: {1}", state, string.Concat(tokens.Select(t => $"{t.Text}({t.Type})")));
    }
}