namespace Shipwreck.VB6Models.Forms
{
    public sealed class Form : ContainerControl
    {
        public string Caption
        {
            get => GetPropertyAsString();
            set => SetProperty(value);
        }

        // 1  '固定(実線)
        public int? BorderStyle
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? ClientWidth
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? ClientHeight
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? ClientLeft
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? ClientTop
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public string LinkTopic
        {
            get => GetPropertyAsString();
            set => SetProperty(value);
        }

        public bool? MaxButton
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public bool? MinButton
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public int? StartUpPosition
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }
    }
}