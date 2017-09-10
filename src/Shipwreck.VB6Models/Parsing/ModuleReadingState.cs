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
                        // TODO: split by comma
                        _Module.Declarations.Add(cd);

                        return true;
                    }
                    else if (ft.IsKeywordOf("Public") || ft.IsKeywordOf("Private") || ft.IsKeywordOf("Dim"))
                    {
                        var i = 1;
                        if (TryReadParameters(tokens, ref i, false, out var ps))
                        {
                            var isPublic = char.ToLowerInvariant(ft.Text[1]) == 'u';

                            foreach (var p in ps)
                            {
                                _Module.Declarations.Add(new FieldDeclaration()
                                {
                                    IsPublic = isPublic,
                                    Name = p.Name,
                                    Type = p.ParameterType
                                });
                            }

                            return true;
                        }
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

            if (TryReadParameters(tokens, ref i, false, out var ps)
                && ps.Count == 1)
            {
                constantDeclaration.Name = ps[0].Name;
                constantDeclaration.Type = ps[0].ParameterType;

                var eq = tokens.ElementAtOrDefault(++i);

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
            }

            constantDeclaration = null;
            return false;
        }

        private bool TryReadParameters(IReadOnlyList<Token> tokens, ref int index, bool allowBy, out List<ParameterDeclaration> parameters)
        {
            var i = index;
            List<ParameterDeclaration> ret = null;
            var last = index;

            for (; ; )
            {
                // (ByVal|ByRef)? id\{suffix} (\(...........\))? (As TypeName)?

                bool? isByRef = null;
                string name;

                var id = tokens.ElementAtOrDefault(i);

                if (allowBy && (id?.IsKeywordOf("ByVal") ?? id?.IsKeywordOf("ByRef") ?? false))
                {
                    isByRef = char.ToLower(id.Text[2]) == 'r';
                    id = tokens.ElementAtOrDefault(++i);
                }

                if (id?.Type != TokenType.Identifier)
                {
                    break;
                }

                ITypeReference elemType = null;

                if (id.Text.Last().IsTypeSuffix())
                {
                    name = id.Text.Substring(0, id.Text.Length - 1);
                    elemType = id.Text.Last().TypeFromSuffix();
                }
                else
                {
                    name = id.Text;
                }

                var isArray = false;

                var comma = tokens.ElementAtOrDefault(++i);
                if (comma.IsOperatorOf("("))
                {
                    // TODO: array dimension
                    var next = tokens.ElementAtOrDefault(++i);
                    if (next.IsOperatorOf(")"))
                    {
                        isArray = true;
                        comma = tokens.ElementAtOrDefault(++i);
                    }
                    else
                    {
                        break;
                    }
                }

                if (elemType == null && comma.IsKeywordOf("As"))
                {
                    var typeName = tokens.ElementAtOrDefault(++i);
                    if (typeName?.Type == TokenType.Identifier)
                    {
                        elemType = typeName.Text.TypeFromName();
                        comma = tokens.ElementAtOrDefault(++i);
                    }
                    else
                    {
                        break;
                    }
                }

                (ret ?? (ret = new List<ParameterDeclaration>())).Add(new ParameterDeclaration()
                {
                    IsByRef = isByRef,
                    Name = name,
                    ParameterType = isArray ? new ArrayType(elemType) : elemType
                });
                last = i - 1;

                if (comma.IsOperatorOf(","))
                {
                    i++;
                    continue;
                }
                else
                {
                    break;
                }
            }

            parameters = ret;
            index = last;

            return ret != null;
        }
    }
}