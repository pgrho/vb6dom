using System.Collections.Generic;

namespace Shipwreck.VB6Models.Forms
{
    public abstract class ContainerControl : Control
    {
        public int? ScaleWidth
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        public int? ScaleHeight
        {
            get => GetPropertyAsInt32();
            set => SetProperty(value);
        }

        private List<Control> _Controls;

        public List<Control> Controls
            => _Controls ?? (_Controls = new List<Control>());
    }
}