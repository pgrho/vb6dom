using System.Linq;
using Shipwreck.VB6Models.Forms;
using Xunit;

namespace Shipwreck.VB6Models.Parsing
{
    public class FormReadingStateTest
    {
        [Fact]
        public void Form1Test()
        {
            var sfr = new SourceFileReader("Parsing\\Form1.frm");
            sfr.Load();

            Assert.Equal("5.00", sfr.Version);

            var frm = Assert.IsType<Form>(sfr.FormObjects.Single());

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