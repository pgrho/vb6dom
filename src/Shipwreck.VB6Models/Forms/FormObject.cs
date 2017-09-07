using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shipwreck.VB6Models.Forms
{
    public abstract class FormObject
    {
        #region Properties

        private Dictionary<string, string> _Properties;

        public Dictionary<string, string> Properties
            => _Properties ?? (_Properties = new Dictionary<string, string>());

        public string GetProperty([CallerMemberName]string name = null)
            => _Properties != null && _Properties.TryGetValue(name, out var v) ? v : null;

        public void SetProperty(string value, [CallerMemberName]string name = null)
        {
            if (value != null)
            {
                Properties[name] = value;
            }
            else
            {
                _Properties?.Remove(name);
            }
        }

        public int? GetPropertyAsInt32([CallerMemberName]string name = null)
            => int.TryParse(GetProperty(name), out var i) ? (int?)i : null;

        public float? GetPropertyAsIntSingle([CallerMemberName]string name = null)
            => float.TryParse(GetProperty(name), out var f) ? (float?)f : null;

        public bool? GetPropertyAsIntBoolean([CallerMemberName]string name = null)
            => int.TryParse(GetProperty(name), out var b) ? (bool?)(b != 0) : null;

        public void SetProperty(int? value, [CallerMemberName]string name = null)
            => SetProperty(value?.ToString("D"), name);

        public void SetProperty(float? value, [CallerMemberName]string name = null)
            => SetProperty(value?.ToString("R"), name);

        public void SetProperty(bool? value, [CallerMemberName]string name = null)
            => SetProperty(value == null ? null : value.Value ? "-1" : "0", name);

        #endregion Properties

        #region BinaryResources

        private Dictionary<string, byte[]> _BinaryResources;

        public Dictionary<string, byte[]> BinaryResources
            => _BinaryResources ?? (_BinaryResources = new Dictionary<string, byte[]>());

        public byte[] GetResource([CallerMemberName]string name = null)
            => _BinaryResources != null && _BinaryResources.TryGetValue(name, out var v) ? v : null;

        #endregion BinaryResources

        #region ObjectProperties

        private Dictionary<string, FormObject> _ObjectProperties;

        public Dictionary<string, FormObject> ObjectProperties
            => _ObjectProperties ?? (_ObjectProperties = new Dictionary<string, FormObject>());

        public FormObject GetPropertyObject([CallerMemberName]string name = null)
        {
            if (!ObjectProperties.TryGetValue(name, out var obj))
            {
                obj = CreatePropertyObject(name);
                ObjectProperties[name] = obj;
            }
            return obj;
        }

        protected virtual FormObject CreatePropertyObject(string name)
        {
            switch (name)
            {
                case "Font":
                    return new Font();
            }

            return new UnknownProperty();
        }

        #endregion ObjectProperties
    }
}