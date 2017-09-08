namespace Shipwreck.VB6Models.Forms
{
    public sealed class Font : FormObject
    {
        public string Name
        {
            get => GetPropertyAsString();
            set => SetProperty(value);
        }

        public int? Charset
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? Weight
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public bool? Underline
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public bool? Italic
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public bool? Strikethrough
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }
    }
}