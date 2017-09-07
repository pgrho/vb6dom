namespace Shipwreck.VB6Models.Forms
{
    [FormType("MSComctlLib.StatusBar")]
    public sealed class StatusBar : Control
    {
        // 2  '下揃え
        public int? ForeColor
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public bool? ShowTips
        {
            get => GetPropertyAsIntBoolean();
            set => SetProperty(value);
        }

        public StatusBarPanelCollection Panels
            => (StatusBarPanelCollection)GetPropertyObject();

        protected override FormObject CreatePropertyObject(string name)
        {
            if (name == nameof(Panels))
            {
                return new StatusBarPanelCollection();
            }
            return base.CreatePropertyObject(name);
        }
    }
}