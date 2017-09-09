namespace Shipwreck.VB6Models.Forms
{
    public sealed class PictureBox : ContainerControl
    {
        public byte[] Picture
        {
            get => GetProperty() as byte[];
            set => SetProperty(value);
        }
    }
}