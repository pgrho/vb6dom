namespace Shipwreck.VB6Models.Declarations
{
    public static class VB6Types
    {
        #region Boolean

        private static BuiltinType _Boolean;

        public static ITypeReference Boolean => _Boolean ?? (_Boolean = new BuiltinType("Boolean"));

        #endregion Boolean

        #region Integer

        private static BuiltinType _Integer;

        public static ITypeReference Integer => _Integer ?? (_Integer = new BuiltinType("Integer"));

        #endregion Integer

        #region Long

        private static BuiltinType _Long;

        public static ITypeReference Long => _Long ?? (_Long = new BuiltinType("Long"));

        #endregion Long

        #region Single

        private static BuiltinType _Single;

        public static ITypeReference Single => _Single ?? (_Single = new BuiltinType("Single"));

        #endregion Single

        #region Double

        private static BuiltinType _Double;

        public static ITypeReference Double => _Double ?? (_Double = new BuiltinType("Double"));

        #endregion Double

        #region Currency

        private static BuiltinType _Currency;

        public static ITypeReference Currency => _Currency ?? (_Currency = new BuiltinType("Currency"));

        #endregion Currency

        #region String

        private static BuiltinType _String;

        public static ITypeReference String => _String ?? (_String = new BuiltinType("String"));

        #endregion String

        #region Date

        private static BuiltinType _Date;

        public static ITypeReference Date => _Date ?? (_Date = new BuiltinType("Date"));

        #endregion Date

        #region Object

        private static BuiltinType _Object;

        public static ITypeReference Object => _Object ?? (_Object = new BuiltinType("Object"));

        #endregion Object

        #region Variant

        private static BuiltinType _Variant;

        public static ITypeReference Variant => _Variant ?? (_Variant = new BuiltinType("Variant"));

        #endregion Variant

        public static ITypeReference FromName(this string type)
        {
            if (type.EqualsIgnoreCase("Boolean"))
            {
                return Boolean;
            }
            if (type.EqualsIgnoreCase("Integer"))
            {
                return Integer;
            }
            if (type.EqualsIgnoreCase("Long"))
            {
                return Long;
            }
            if (type.EqualsIgnoreCase("Single"))
            {
                return Single;
            }
            if (type.EqualsIgnoreCase("Double"))
            {
                return Double;
            }
            if (type.EqualsIgnoreCase("Currency"))
            {
                return Currency;
            }
            if (type.EqualsIgnoreCase("String"))
            {
                return String;
            }
            if (type.EqualsIgnoreCase("Date"))
            {
                return Date;
            }
            if (type.EqualsIgnoreCase("Object"))
            {
                return Object;
            }
            if (type.EqualsIgnoreCase("Variant"))
            {
                return Variant;
            }
            return null;
        }
    }
}