using System.Linq;
using Shipwreck.VB6Models.Declarations;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    public class ModuleReadingStateTest
    {
        private static StandardModule _Module1;

        private static StandardModule Module1 => _Module1 ?? (_Module1 = Assert.IsType<StandardModule>(new SourceFileReader("Parsing\\module1.bas").Load()));

        #region ConstMatcher


        #endregion

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

        #endregion Const

        #region Dim

        [Fact]
        public void Module1Test_Dim1()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var1");
        }

        [Fact]
        public void Module1Test_Dim2()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var2");
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
        }

        [Fact]
        public void Module1Test_Dim5()
        {
            var cnst = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var5");
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
            var v3 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_3");
            var v4 = Module1.Declarations.OfType<FieldDeclaration>().Single(c => c.Name == "var8_4");
        }

        #endregion Dim
    }
}