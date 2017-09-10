using System.Collections.Generic;
using System.Linq;
using Shipwreck.VB6Models.Declarations;
using Shipwreck.VB6Models.Expressions;
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
                    else if (tokens.Any(t => t.IsKeywordOf("Declare")))
                    {
                    }
                    else if (tokens.Any(t => t.Type == TokenType.Keyword && t.Text.EqualsIgnoreCase("Sub", "Function", "Property")))
                    {
                    }
                    else if (TryCreateConstantDeclaration(tokens, out var cd))
                    {
                        _Module.Declarations.Add(cd);
                    }
                    else
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

        private bool TryCreateConstantDeclaration(IReadOnlyList<Token> tokens, out ConstantDeclaration constantDeclaration)
        {
            var ft = tokens.First();

            if (tokens.Count < 4 && !tokens.Any(t => t.IsKeywordOf("Const")))
            {
                constantDeclaration = null;
                return false;
            }
            constantDeclaration = new ConstantDeclaration();
            constantDeclaration.IsPublic = ft.IsKeywordOf("Public") ? true
                                        : ft.IsKeywordOf("Private") ? false
                                        : (bool?)null;

            var i = constantDeclaration.IsPublic == null ? 0 : 1;

            if (!tokens[i++].IsKeywordOf("Const"))
            {
                constantDeclaration = null;
                return false;
            }

            var id = tokens[i++];

            if (id.Type != TokenType.Identifier)
            {
                constantDeclaration = null;
                return false;
            }

            if (id.Text.Last().IsTypeSuffix())
            {
                constantDeclaration.Name = id.Text.Substring(0, id.Text.Length - 1);
                constantDeclaration.Type = id.Text.Last().TypeFromSuffix();
            }
            else
            {
                constantDeclaration.Name = id.Text;
            }

            var eq = tokens[i];

            if (eq.IsKeywordOf("As") && constantDeclaration.Type == null)
            {
                if (i + 1 >= tokens.Count)
                {
                    constantDeclaration = null;
                    return false;
                }

                constantDeclaration.Type = tokens[i + 1].Text.TypeFromName();

                i += 2;

                eq = tokens[i];
            }

            if (eq.IsOperatorOf("=")
                && i + 2 <= tokens.Count
                && i + 3 >= tokens.Count)
            {
                var vt = tokens[i + 1];

                if (vt.IsOperatorOf("-")
                    && i + 3 == tokens.Count)
                {
                    constantDeclaration.Value = new ConstantExpression(tokens[i + 2].GetValue().Negate());
                }
                else if (i + 2 == tokens.Count)
                {
                    constantDeclaration.Value = new ConstantExpression(vt.GetValue());
                }

                return true;
            }

            constantDeclaration = null;
            return false;
        }
    }
}