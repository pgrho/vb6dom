namespace Shipwreck.VB6Models.Forms
{
    public sealed class PictureBox : ContainerControl
    {
        public byte[] Picture
            => GetResource();
    }
}