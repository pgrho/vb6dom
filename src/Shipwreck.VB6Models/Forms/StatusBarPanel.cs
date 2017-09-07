namespace Shipwreck.VB6Models.Forms
{
    public sealed class StatusBarPanel : FormObject
    {
        // 1
        // 2
        public int? AutoSize
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public string Text
        {
            get => GetProperty();
            set => SetProperty(value);
        }

        public int? MinWidth
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }
    }
}