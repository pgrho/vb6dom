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
        }
    }
}