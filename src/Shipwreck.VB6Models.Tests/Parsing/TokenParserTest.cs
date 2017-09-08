using System.IO;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    public class TokenParserTest
    {
        [Fact]
        public void VersionTest()
            => AssertTokens("VERSION 5.00", ID("VERSION"), FL("5.00"));

        [Fact]
        public void BeginFormTest()
            => AssertTokens("Begin VB.Form Form1", KW("Begin"), ID("VB"), OP("."), ID("Form"), ID("Form1"));

        #region Common

        private Token ID(string text)
            => TOKEN(TokenType.Identifier, text);

        private Token KW(string text)
            => TOKEN(TokenType.Keyword, text);

        private Token OP(string text)
            => TOKEN(TokenType.Operator, text);

        private Token FL(string text)
            => TOKEN(TokenType.Float, text);

        private Token TOKEN(TokenType type, string text)
            => new Token(type, -1, -1, text);

        private void AssertTokens(string source, params Token[] tokens)
        {
            using (var sr = new StringReader(source))
            using (var tp = new TokenParser(sr))
            {
                Assert.True(tp.Read());
                Assert.Equal(tokens.Length, tp.Tokens.Count);

                for (int i = 0; i < tokens.Length; i++)
                {
                    var e = tokens[i];
                    var a = tp.Tokens[i];

                    Assert.Equal(e.Type, a.Type);
                    Assert.Equal(e.Text, a.Text);
                }
            }
        }

        #endregion Common
    }
}