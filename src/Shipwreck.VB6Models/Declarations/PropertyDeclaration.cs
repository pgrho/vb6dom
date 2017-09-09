using System.Collections.Generic;

namespace Shipwreck.VB6Models.Declarations
{

    public sealed class PropertyDeclaration : MethodDeclarationBase
    {
        public bool? IsPublic { get; set; }

        public string Name { get; set; }

        public PropertyAccessorType AccessorType { get; set; }

        public ITypeReference PropertyType { get; set; }
    }
}