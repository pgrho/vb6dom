using System;

namespace Shipwreck.VB6Models.Forms
{
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class FormTypeAttribute : Attribute
    {
        public FormTypeAttribute(string typeName)
        {
            TypeName = TypeName;
        }

        public string TypeName { get; }
    }
}