namespace Shipwreck.VB6Models.Declarations
{
    public sealed class UnknownType : ITypeReference
    {
        public UnknownType(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}