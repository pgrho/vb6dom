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
    }
}