namespace Shipwreck.VB6Models.Forms
{
    public abstract class Control : FormObject
    {
        public string Name { get; set; }

        public int? Left
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? Top
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? Width
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? Height
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? TabIndex
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public Font Font
            => (Font)GetPropertyObject();
    }
}