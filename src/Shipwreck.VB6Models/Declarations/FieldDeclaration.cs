namespace Shipwreck.VB6Models.Declarations
{
    public sealed class FieldDeclaration : Declaration
    {
        public bool? IsPublic { get; set; }

        public string Name { get; set; }

        public ITypeReference Type { get; set; }
    }
}