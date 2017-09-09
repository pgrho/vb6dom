using System.Collections.Generic;
using System.Linq;
using Shipwreck.VB6Models.Declarations;
using Shipwreck.VB6Models.Forms;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class ModuleReadingState : ISourceReadingState
    {
        private readonly ModuleBase _Module;

        public ModuleReadingState(ModuleBase module)
        {
            _Module = module;
        }

        public bool Accept(SourceFileReader reader, IReadOnlyList<Token> tokens)
        {
            var ft = tokens.First();

            switch (ft.Type)
            {
                case TokenType.Identifier:
                    if ("VERSION".EqualsIgnoreCase(ft.Text))
                    {
                        if (tokens.Count == 2)
                        {
                            _Module.Version = tokens[1].Text;
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
                        ((FormModule)_Module).Form = (Form)c;
                        reader.Push(new FormReadingState(c));
                        return true;
                    }
                    else if (tokens.Any(t => t.Type == TokenType.Keyword && t.Text.EqualsIgnoreCase("Declare")))
                    {
                    }
                    else if (tokens.Any(t => t.Type == TokenType.Keyword && t.Text.EqualsIgnoreCase("Sub", "Function", "Property")))
                    {
                    }

                    break;

                case TokenType.Comment:
                    _Module.Declarations.Add(new DeclarationComment()
                    {
                        Comment = ft.Text.Substring(1)
                    });
                    return true;
            }

            reader.OnUnknownTokens(this, tokens);

            return true;
        }
    }
}