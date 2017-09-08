namespace Shipwreck.VB6Models.Forms
{
    public sealed class Label : Control
    {
        // 2  '中央揃え
        public int? Alignment
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public bool? AutoSize
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public Color? BackColor
        {
            get => GetPropertyAsIntColor();
            set => SetProperty(value);
        }

        // 1  '実線
        public int? BorderStyle
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public string Caption
        {
            get => GetPropertyAsString();
            set => SetProperty(value);
        }

        public Color? ForeColor
        {
            get => GetPropertyAsIntColor();
            set => SetProperty(value);
        }
    }
}