namespace Shipwreck.VB6Models.Forms
{
    [FormType("VB.PictureBox")]
    public sealed class PictureBox : ContainerControl
    {
        public byte[] Picture
            => GetResource();
    }
}