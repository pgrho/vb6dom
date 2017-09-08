using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shipwreck.VB6Models.Forms;

namespace Shipwreck.VB6Models.Parsing
{
    internal sealed class FormReadingState : ISourceReadingState
    {
        private static Dictionary<string, Type> _FormObjectTypes = new Dictionary<string, Type>
        {
            ["VB.Form"] = typeof(Form),
            ["VB.CommandButton"] = typeof(CommandButton),
            ["VB.Label"] = typeof(Label),
            ["VB.PictureBox"] = typeof(PictureBox),
            ["MSComctlLib.StatusBar"] = typeof(StatusBar)
        };

        private readonly FormObject _Object;
        private readonly bool _IsControl;

        public FormReadingState(FormObject @object)
        {
            _Object = @object;
            _IsControl = @object is Control;
        }

        public static FormObject Create(string typeName)
        {
            if (_FormObjectTypes.TryGetValue(typeName, out var type))
            {
                return (FormObject)Activator.CreateInstance(type);
            }
            return null;
        }

        internal static bool TryCreateControl(IReadOnlyList<Token> tokens, out Control control)
        {
            var ft = tokens.First();

            if (ft.Type == TokenType.Keyword && ft.Text.EqualsIgnoreCase("Begin"))
            {
                for (var i = 1; i + 1 < tokens.Count; i += 2)
                {
                    var it = tokens[i];
                    var nt = tokens[i + 1];
                    if (it.Type == TokenType.Identifier)
                    {
                        if (nt.Type == TokenType.Identifier)
                        {
                            var tn = string.Concat(tokens.Skip(1).Take(i).Select(t => t.Text));

                            control = Create(tn) as Control ?? new UnknownControl()
                            {
                                TypeName = tn
                            };

                            return true;
                        }
                        else if (nt.Type == TokenType.Operator && nt.Text == ".")
                        {
                            continue;
                        }
                    }
                    break;
                }
            }

            control = null;
            return false;
        }

        public bool Accept(SourceFileReader reader, IReadOnlyList<Token> tokens)
        {
            var ft = tokens.First();

            switch (ft.Type)
            {
                case TokenType.Keyword:
                    if (_Object is ContainerControl && TryCreateControl(tokens, out var c))
                    {
                        ((ContainerControl)_Object).Controls.Add(c);
                        reader.Push(new FormReadingState(c));
                        return true;
                    }
                    else if (ft.Text.EqualsIgnoreCase("BeginProperty"))
                    {
                        if (tokens.Count >= 2)
                        {
                            var nt = tokens[1];
                            if (nt.Type == TokenType.Identifier)
                            {
                                var obj = _Object.GetPropertyObject(nt.Text);
                                reader.Push(new FormReadingState(obj));
                                return true;
                            }
                        }
                    }
                    else if (ft.Text.EqualsIgnoreCase("End"))
                    {
                        if (_IsControl)
                        {
                            return true;
                        }
                    }
                    else if (ft.Text.EqualsIgnoreCase("EndProperty"))
                    {
                        if (!_IsControl)
                        {
                            return true;
                        }
                    }
                    break;
            }

            reader.OnUnknownTokens(this, tokens);

            return true;
        }
    }
}