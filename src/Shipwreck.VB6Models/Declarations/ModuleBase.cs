using System.Collections.Generic;

namespace Shipwreck.VB6Models.Declarations
{
    public abstract class ModuleBase : ITypeReference
    {
        public string Version { get; set; }

        private List<Declaration> _Declarations;

        public List<Declaration> Declarations
            => _Declarations ?? (_Declarations = new List<Declaration>());
    }
}