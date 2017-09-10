using Shipwreck.VB6Models.Expressions;

namespace Shipwreck.VB6Models.Declarations
{
    public sealed class ConstantDeclaration : Declaration
    {
        public bool? IsPublic { get; set; }

        public string Name { get; set; }

        public ITypeReference Type { get; set; }

        public Expression Value { get; set; }
    }
}