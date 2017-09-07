namespace Shipwreck.VB6Models.Forms
{
    public sealed class StatusBarPanelCollection : FormObject
    {
        public StatusBarPanel this[int index]
            => (StatusBarPanel)GetPropertyObject("Panel" + (index + 1));

        public int? NumPanels
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        protected override FormObject CreatePropertyObject(string name)
        {
            if (name.StartsWith("Panel")
                && int.TryParse(name.Substring(5), out var i))
            {
                return new StatusBarPanel();
            }

            return base.CreatePropertyObject(name);
        }
    }
}