using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                            control.Name = nt.Text;

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
                            return false;
                        }
                    }
                    else if (ft.Text.EqualsIgnoreCase("EndProperty"))
                    {
                        if (!_IsControl)
                        {
                            return false;
                        }
                    }
                    break;
            }

            for (var i = 0; i + 2 < tokens.Count; i += 2)
            {
                var it = tokens[i];

                if (it.Type != TokenType.Identifier)
                {
                    break;
                }

                var nt = tokens[i + 1];

                if (nt.Type != TokenType.Operator)
                {
                    break;
                }

                switch (nt.Text)
                {
                    case ".":
                        continue;

                    case "=":

                        var name = string.Concat(tokens.Take(i + 1).Select(t => t.Text));
                        var vt = tokens[i + 2];

                        switch (vt.Type)
                        {
                            case TokenType.Operator:
                                switch (vt.Text)
                                {
                                    case "-":
                                        if (tokens.Count > i + 3
                                            && (tokens[i + 3].Type == TokenType.Integer || tokens[i + 3].Type == TokenType.Float))
                                        {
                                            if (tokens[i + 3].Text != "0"
                                                && tokens.Count == i + 4
                                                && Regex.IsMatch(@"'\s*[Tt]rue\s$", tokens[i + 4].Text))
                                            {
                                                _Object.SetProperty(true, name);
                                            }
                                            else
                                            {
                                                _Object.SetProperty(tokens[i + 3].GetValue().Negate(), name);
                                            }
                                            return true;
                                        }
                                        break;

                                    case "$":
                                        if (tokens.Count >= i + 5
                                            && tokens[i + 3].Type == TokenType.String
                                            && tokens[i + 4].Text == ":"
                                            && int.TryParse(tokens[i + 4].Text, NumberStyles.HexNumber, null, out var h2))
                                        {
                                            // TODO: read binary and to string

                                            _Object.SetProperty((reader.Encoding ?? Encoding.Default).GetString(reader.ReadBinary(tokens[i + 3].Text, h2)), name);

                                            return true;
                                        }
                                        break;
                                }
                                break;

                            case TokenType.String:
                                if (tokens.Count >= i + 4
                                    && tokens[i + 3].Text == ":"
                                    && int.TryParse(tokens[i + 4].Text, NumberStyles.HexNumber, null, out var h))
                                {
                                    _Object.SetProperty(reader.ReadBinary(vt.Text, h), name);

                                    return true;
                                }
                                else
                                {
                                    _Object.SetProperty(vt.Text, name);
                                    return true;
                                }

                            case TokenType.Integer:
                                if (vt.Text == "0"
                                    && tokens.Count == i + 3
                                    && Regex.IsMatch(@"'\s*[Ff]alse\s$", tokens[i + 3].Text))
                                {
                                    _Object.SetProperty(false, name);
                                }
                                else
                                {
                                    _Object.SetProperty(vt.GetValue(), name);
                                }
                                return true;

                            case TokenType.Float:
                                _Object.SetProperty(vt.GetValue(), name);
                                return true;
                        }
                        break;
                }
            }

            reader.OnUnknownTokens(this, tokens);

            return true;
        }
    }
}