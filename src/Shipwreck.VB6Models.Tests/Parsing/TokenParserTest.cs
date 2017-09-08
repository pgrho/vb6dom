using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Xunit.Sdk;

namespace Shipwreck.VB6Models.Parsing
{
    public class TokenParserTest
    {
        #region Unit

        [Fact]
        public void LineContinuationTest()
            => AssertTokens("Dim a _ \r\n As Integer", KW("Dim"), ID("a"), KW("As"), ID("Integer"));

        #endregion Unit

        #region Header Lines

        [Fact]
        public void VersionTest()
            => AssertTokens("VERSION 5.00", ID("VERSION"), FL("5.00"));

        [Fact]
        public void ObjectDeclarationTest()
            => AssertTokens(
                "Object = \"{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0\"; \"MSCOMCTL.OCX\"",
                ID("Object"), OP("="), ST("{831FDD16-0C5C-11D2-A9FC-0000F8754DA1}#2.1#0"), OP(";"), ST("MSCOMCTL.OCX"));

        [Fact]
        public void StringAttributeTest()
            => AssertTokens("Attribute VB_Name = \"Form1\"", ID("Attribute"), ID("VB_Name"), OP("="), ST("Form1"));

        [Fact]
        public void BooleanAttributeTest()
            => AssertTokens("Attribute VB_Exposed = False", ID("Attribute"), ID("VB_Exposed"), OP("="), BL("False"));

        #endregion Header Lines

        #region Form Designer

        [Fact]
        public void BeginFormTest()
            => AssertTokens("Begin VB.Form Form1", KW("Begin"), ID("VB"), OP("."), ID("Form"), ID("Form1"));

        [Fact]
        public void IntegerPropertyTest()
            => AssertTokens("  ClientHeight  =   4620", ID("ClientHeight"), OP("="), IN("4620"));

        [Fact]
        public void NegativeIntegerTest()
            => AssertTokens("  Top  =   -1234", ID("Top"), OP("="), OP("-"), IN("1234"));

        [Fact]
        public void StringPropertyTest()
            => AssertTokens("  Caption  =   \"Form Caption\"", ID("Caption"), OP("="), ST("Form Caption"));

        [Fact]
        public void FalsePropertyTest()
            => AssertTokens("  MaxButton  =   0 'False", ID("MaxButton"), OP("="), IN("0"), CM("'False"));

        [Fact]
        public void TruePropertyTest()
            => AssertTokens("  MinButton  =   -1 'True", ID("MinButton"), OP("="), OP("-"), IN("1"), CM("'True"));

        [Fact]
        public void BeginPropertyTest()
            => AssertTokens("  BeginProperty Font", ID("BeginProperty"), ID("Font"));

        [Fact]
        public void BeginPropertyGuidTest()
            => AssertTokens(
                "  BeginProperty Panels {8E3867A5-8586-11D1-B16A-00C0F0283628}",
                ID("BeginProperty"), ID("Panels"), UU("8E3867A5-8586-11D1-B16A-00C0F0283628"));

        [Fact]
        public void ObjectWidthTest()
            => AssertTokens("  Object.Width           =   1764",
                ID("Object"), OP("."), ID("Width"), OP("="), IN("1764"));

        [Fact]
        public void PrivatePropertyTest()
            => AssertTokens("  _ExtentX           =   1764",
                ID("_ExtentX"), OP("="), IN("1764"));

        [Fact]
        public void ColorPropertyTest()
            => AssertTokens("  BackColor  =   &HABCDEF01&",
                ID("BackColor"), OP("="), IN("&HABCDEF01&"));

        [Fact]
        public void FrxPropertyTest()
            => AssertTokens("  Picture =  \"Form1.frx\":ABCDEF",
                ID("Picture"), OP("="), ST("Form1.frx"), OP(":"), ID("ABCDEF"));

        [Fact]
        public void FrxStringPropertyTest()
            => AssertTokens("  Picture =  $\"Form1.frx\":0123456",
                ID("Picture"), OP("="), OP("$"), ST("Form1.frx"), OP(":"), IN("0123456"));

        #endregion Form Designer

        #region Declaration

        [Fact]
        public void OptionExplicitTest()
            => AssertTokens("Option Explicit", KW("Option"), ID("Explicit"));

        [Fact]
        public void MethodTest()
            => AssertLines(
                "Private Sub Main()\r\nEnd Sub",
                new[]
                {
                    new[] { KW("Private"), KW("Sub"), ID("Main"), OP("("), OP(")") },
                    new[] { KW("End"), KW("Sub") }
                });

        #endregion Declaration

        #region Common

        private Token ID(string text)
            => TOKEN(TokenType.Identifier, text);

        private Token KW(string text)
            => TOKEN(TokenType.Keyword, text);

        private Token OP(string text)
            => TOKEN(TokenType.Operator, text);

        private Token ST(string text)
            => TOKEN(TokenType.String, text);

        private Token IN(string text)
            => TOKEN(TokenType.Integer, text);

        private Token FL(string text)
            => TOKEN(TokenType.Float, text);

        private Token BL(string text)
            => TOKEN(TokenType.Boolean, text);

        private Token UU(string text)
            => TOKEN(TokenType.Guid, text);

        private Token CM(string text)
            => TOKEN(TokenType.Comment, text);

        private Token TOKEN(TokenType type, string text)
            => new Token(type, -1, -1, text);

        private void AssertTokens(string source, params Token[] tokens)
        {
            using (var sr = new StringReader(source))
            using (var tp = new TokenParser(sr))
            {
                AssertLine(tokens, tp);

                Assert.False(tp.Read());
            }
        }

        private void AssertLines(string source, params Token[][] lines)
        {
            using (var sr = new StringReader(source))
            using (var tp = new TokenParser(sr))
            {
                foreach (var line in lines)
                {
                    AssertLine(line, tp);
                }

                Assert.False(tp.Read());
            }
        }

        private static void AssertLine(Token[] tokens, TokenParser tp)
        {
            Assert.True(tp.Read());

            try
            {
                Assert.Equal(tokens.Length, tp.Tokens.Count);

                for (int i = 0; i < tokens.Length; i++)
                {
                    var e = tokens[i];
                    var a = tp.Tokens[i];

                    Assert.Equal(e.Type, a.Type);
                    Assert.Equal(e.Text, a.Text);
                }
            }
            catch
            {
                string toString(IEnumerable<Token> toks) => string.Join(", ", toks.Select(t => $"{t.Text}({t.Type})"));

                throw new EqualException(toString(tokens), toString(tp.Tokens));
            }
        }

        #endregion Common
    }
}