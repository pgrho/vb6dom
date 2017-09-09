namespace Shipwreck.VB6Models.Declarations
{
    public sealed class ParameterDeclaration
    {
        public bool? IsByRef { get; set; }

        public string Name { get; set; }

        public ITypeReference ParameterType { get; set; }
    }
}