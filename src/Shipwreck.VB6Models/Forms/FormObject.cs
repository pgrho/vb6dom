using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Shipwreck.VB6Models.Forms
{
    public abstract class FormObject
    {
        #region Properties

        private Dictionary<string, object> _Properties;

        public Dictionary<string, object> Properties
            => _Properties ?? (_Properties = new Dictionary<string, object>());

        public object GetProperty([CallerMemberName]string name = null)
            => _Properties != null && _Properties.TryGetValue(name, out var v) ? v : null;

        public void SetProperty(object value, [CallerMemberName]string name = null)
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

        public string GetPropertyAsString([CallerMemberName]string name = null)
            => GetProperty(name)?.ToString();

        public int? GetPropertyAsInt32([CallerMemberName]string name = null)
        {
            var v = GetProperty(name);
            return v is int i ? i
                    : v is IConvertible c && !(v is string) ? c.ToInt32(null)
                    : int.TryParse(v?.ToString(), out i) ? (int?)i : null;
        }

        public float? GetPropertyAsIntSingle([CallerMemberName]string name = null)
        {
            var v = GetProperty(name);
            return v is float f ? f
                    : v is IConvertible c && !(v is string) ? c.ToSingle(null)
                    : float.TryParse(v?.ToString(), out f) ? (float?)f : null;
        }

        public bool? GetPropertyAsIntBoolean([CallerMemberName]string name = null)
        {
            var v = GetProperty(name);
            return v is bool b ? b
                    : v is IConvertible c && !(v is string) ? c.ToSingle(null) != 0
                    : float.TryParse(v?.ToString(), out var f) ? (f != 0)
                    : bool.TryParse(v?.ToString(), out b) ? (bool?)b : null;
        }

        public Color? GetPropertyAsIntColor([CallerMemberName]string name = null)
            => GetProperty(name) is Color f ? (Color?)f : null;

        public void SetProperty(bool? value, [CallerMemberName]string name = null)
            => SetProperty(value == null ? null : value.Value ? (int?)-1 : 0, name);

        #endregion Properties

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