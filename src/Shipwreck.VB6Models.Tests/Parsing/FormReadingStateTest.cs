using System.Linq;
using Shipwreck.VB6Models.Declarations;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    public class FormReadingStateTest
    {
        #region Form1

        private static FormModule _Form1;

        private static FormModule Form1 => _Form1 ?? (_Form1 = Assert.IsType<FormModule>(new SourceFileReader("Parsing\\Form1.frm").Load()));

        [Fact]
        public void Form1Test_Form()
        {
            Assert.Equal("5.00", Form1.Version);

            var frm = Form1.Form;

            Assert.Equal("Form1", frm.Name);
            Assert.Equal("Form Title", frm.Caption);
            Assert.Equal(4170, frm.ClientWidth);
            Assert.Equal(1905, frm.ClientHeight);
            Assert.Equal(45, frm.ClientLeft);
            Assert.Equal(330, frm.ClientTop);

            Assert.Equal("MS UI Gothic", frm.Font.Name);
        }

        [Fact]
        public void Form1Test_Const1()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const1");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Form1Test_Const2()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const2");
            Assert.Equal(false, cnst.IsPublic);
        }

        [Fact]
        public void Form1Test_Const3()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const3");
            Assert.Equal(true, cnst.IsPublic);
        }

        [Fact]
        public void Form1Test_Const4()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const4");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Form1Test_Const5()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const5");
            Assert.Null(cnst.IsPublic);
        }

        [Fact]
        public void Form1Test_Const6()
        {
            var cnst = Form1.Declarations.OfType<ConstantDeclaration>().Single(c => c.Name == "const6");
            Assert.Null(cnst.IsPublic);
        }

        #endregion Form1
    }
}