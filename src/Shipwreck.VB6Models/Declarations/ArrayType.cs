namespace Shipwreck.VB6Models.Declarations
{
    public sealed class ArrayType : ITypeReference
    {
        public ArrayType(ITypeReference elementType)
        {
            ElementType = elementType;
        }

        public ITypeReference ElementType { get; }
    }
}