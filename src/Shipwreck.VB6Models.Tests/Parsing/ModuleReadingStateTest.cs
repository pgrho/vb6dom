using System.Linq;
using Shipwreck.VB6Models.Declarations;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    using static TokenFactory;

    public class ModuleReadingStateTest
    {
        private static StandardModule _Module1;

        private static StandardModule Module1 => _Module1 ?? (_Module1 = Assert.IsType<StandardModule>(new SourceFileReader("Parsing\\module1.bas").Load()));

        #region ConstMatcher

        [Fact]
        public void Const1Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Const"), ID("const1"), OP("="), IN("1") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.Null(c.IsPublic);
            Assert.Equal("const1", c.Name);
            Assert.Null(c.Type);
        }

        [Fact]
        public void Const2Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Private"), KW("Const"), ID("const2"), OP("="), IN("2") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.False(c.IsPublic);
            Assert.Equal("const2", c.Name);
            Assert.Null(c.Type);
        }

        [Fact]
        public void Const3Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Public"), KW("Const"), ID("const3"), OP("="), IN("2") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.True(c.IsPublic);
            Assert.Equal("const3", c.Name);
            Assert.Null(c.Type);
        }

        [Fact]
        public void Const4Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Const"), ID("const4&"), OP("="), IN("2") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.Null(c.IsPublic);
            Assert.Equal("const4", c.Name);
            Assert.Equal(VB6Types.Long, c.Type);
        }

        [Fact]
        public void Const5Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Const"), ID("const5"), KW("As"), ID("Double"), OP("="), IN("2") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.Null(c.IsPublic);
            Assert.Equal("const5", c.Name);
            Assert.Equal(VB6Types.Double, c.Type);
        }

        [Fact]
        public void Const6Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] { KW("Const"), ID("const6"), KW("As"), ID("Integer"), OP("="), OP("-"), IN("6") }, sm));
            var c = Assert.IsType<ConstantDeclaration>(sm.Declarations.Single());

            Assert.Null(c.IsPublic);
            Assert.Equal("const6", c.Name);
            Assert.Equal(VB6Types.Integer, c.Type);
        }

        [Fact]
        public void Const7to9Test()
        {
            var sm = new StandardModule();

            Assert.True(ModuleReadingState.ConstMatcher.TryMatch(new[] {
                KW("Const"), ID("const7"), KW("As"), ID("Integer"), OP("="), IN("7"),
                OP(","), ID("const8"), OP("="), IN("8"),
                OP(","), ID("const9@"), OP("="), IN("9")
            }, sm));

            {
                var c = sm.Declarations.OfType<ConstantDeclaration>().Single(e => e.Name == "const7");

                Assert.Null(c.IsPublic);
                Assert.Equal("const7", c.Name);
                Assert.Equal(VB6Types.Integer, c.Type);
            }
            {
                var c = sm.Declarations.OfType<ConstantDeclaration>().Single(e => e.Name == "const8");

                Assert.Null(c.IsPublic);
                Assert.Equal("const8", c.Name);
                Assert.Equal(null, c.Type);
            }
            {
                var c = sm.Declarations.OfType<ConstantDeclaration>().Single(e => e.Name == "const9");

                Assert.Null(c.IsPublic);
                Assert.Equal("const9", c.Name);
                Assert.Equal(VB6Types.Currency, c.Type);
            }
        }

        #endregion ConstMatcher

        #region Const

        [Fact]
        public void Module1Test_Const1()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const1");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const2()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const2");
            Assert.Equal(false, cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const3()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const3");
            Assert.Equal(true, cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const4()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const4");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const5()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const5");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const6()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const6");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const7()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const7");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const8()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const8");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Const9()
        {
            var cnst = Module1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const9");
            Assert.Null(cnst.IsPublic);
        }

        #endregion Const

        #region Dim

        [Fact]
        public void Module1Test_Dim1()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var1");
            Assert.True(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Dim2()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var2");
            Assert.False(cnst.IsPublic);
        }

        [Fact]
        public void Module1Test_Dim3()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var3");
        }

        [Fact]
        public void Module1Test_Dim4()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var4");
            Assert.Equal(VB6Types.Single, cnst.Type);
        }

        [Fact]
        public void Module1Test_Dim5()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var5");
            Assert.Equal(VB6Types.String, cnst.Type);
        }

        [Fact]
        public void Module1Test_Dim6()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var6");
        }

        [Fact]
        public void Module1Test_Dim7()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var7");
        }

        [Fact]
        public void Module1Test_Dim8()
        {
            var v1 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_1");
            var v2 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_2");
            Assert.Equal(VB6Types.Currency, v2.Type);
            var v3 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_3");
            var v4 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_4");
            Assert.Equal(VB6Types.Integer, v4.Type);
        }

        #endregion Dim
    }
}