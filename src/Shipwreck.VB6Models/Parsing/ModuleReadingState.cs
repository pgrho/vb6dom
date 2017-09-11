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

        #region ConstMatcher

        private static TokenMatcher<ModuleBase> _ConstMatcher;

        internal static TokenMatcher<ModuleBase> ConstMatcher
            => _ConstMatcher
            ?? (_ConstMatcher
                    = new TokenMatcherBuilder()
                            .ContinueWithOptionalKeyword("isPublic", t => t.Text[1] == 'u' || t.Text[1] == 'U', "Public", "Private")
                            .ContinueWithKeyword("Const")
                            .ContinueWithMany("b",
                                g => g.ContinueWithIdentifier("id")
                                        .ContinueWithOptionalGroup(g2 => g2.ContinueWithKeyword("As").ContinueWithIdentifier("typeName"))
                                        .ContinueWithOperator("=")
                                        .ContinueWithExpression("v"),
                                ",",
                                s =>
                                {
                                    var cd = new ConstantDeclaration();

                                    ITypeReference elemType = null;
                                    cd.Name = s.Captures["id"].ToString();

                                    if (cd.Name.Last().IsTypeSuffix())
                                    {
                                        elemType = cd.Name.Last().TypeFromSuffix();
                                        cd.Name = cd.Name.Substring(0, cd.Name.Length - 1);
                                    }
                                    else if (s.Captures.TryGetValue("typeName", out var tn))
                                    {
                                        var t = tn.ToString();
                                        elemType = t.TypeFromName() ?? new UnknownType(t);
                                    }
                                    cd.Type = elemType;

                                    cd.Value = (Expression)s.Captures["v"];

                                    return cd;
                                })
                            // TODO: capture comment
                            .ToMatcher<ModuleBase>((s, mb) =>
                            {
                                var isPublic = s.Captures.TryGetValue("isPublic", out var b) ? true.Equals(b) : (bool?)null;

                                foreach (var cd in (IEnumerable<ConstantDeclaration>)s.Captures["b"])
                                {
                                    cd.IsPublic = isPublic;

                                    mb.Declarations.Add(cd);
                                }
                            }));

        #endregion ConstMatcher

        #region FieldMatcher

        private static TokenMatcher<ModuleBase> _FieldMatcher;

        internal static TokenMatcher<ModuleBase> FieldMatcher
            => _FieldMatcher
            ?? (_FieldMatcher
                    = new TokenMatcherBuilder()
                            .ContinueWithKeyword("isPublic", t => t.Text[1] == 'u' || t.Text[1] == 'U', "Public", "Private", "Dim")
                            .ContinueWithMany("b",
                                g => g.ContinueWithIdentifier("id")
                                        .ContinueWithOptionalGroup("isArray", g2 => g2.ContinueWithOperator("(").ContinueWithOperator(")"))
                                        .ContinueWithOptionalGroup(g2 => g2.ContinueWithKeyword("As").ContinueWithIdentifier("typeName")),
                                ",",
                                s =>
                                {
                                    var cd = new FieldDeclaration();

                                    ITypeReference elemType = null;
                                    cd.Name = s.Captures["id"].ToString();

                                    if (cd.Name.Last().IsTypeSuffix())
                                    {
                                        elemType = cd.Name.Last().TypeFromSuffix();
                                        cd.Name = cd.Name.Substring(0, cd.Name.Length - 1);
                                    }
                                    else if (s.Captures.TryGetValue("typeName", out var tn))
                                    {
                                        var t = tn.ToString();
                                        elemType = t.TypeFromName() ?? new UnknownType(t);
                                    }
                                    var isArray = s.Captures.ContainsKey("isArray");
                                    cd.Type = isArray ? new ArrayType(elemType ?? VB6Types.Variant) : elemType;

                                    return cd;
                                })
                            // TODO: capture comment
                            .ToMatcher<ModuleBase>((s, mb) =>
                            {
                                var isPublic = s.Captures.TryGetValue("isPublic", out var b) ? true.Equals(b) : (bool?)null;

                                foreach (var cd in (IEnumerable<FieldDeclaration>)s.Captures["b"])
                                {
                                    cd.IsPublic = isPublic;

                                    mb.Declarations.Add(cd);
                                }
                            }));

        #endregion FieldMatcher

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
                    else if (ConstMatcher.TryMatch(tokens, _Module)
                            || FieldMatcher.TryMatch(tokens, _Module))
                    {
                        return true;
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