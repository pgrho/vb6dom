using Shipwreck.VB6Models.Declarations;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    public class FormReadingStateTest
    {
        [Fact]
        public void Form1Test()
        {
            var sfr = new SourceFileReader("Parsing\\Form1.frm");
            var m = sfr.Load();

            Assert.Equal("5.00", m.Version);

            var frm = Assert.IsType<FormModule>(m).Form;

            Assert.Equal("Form1", frm.Name);
            Assert.Equal("Form Title", frm.Caption);
            Assert.Equal(4170, frm.ClientWidth);
            Assert.Equal(1905, frm.ClientHeight);
            Assert.Equal(45, frm.ClientLeft);
            Assert.Equal(330, frm.ClientTop);

            Assert.Equal("MS UI Gothic", frm.Font.Name);
        }
    }
}