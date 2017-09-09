using System.Collections.Generic;

namespace Shipwreck.VB6Models.Declarations
{
    public abstract class MethodDeclarationBase : Declaration
    {
        private List<ParameterDeclaration> _Parameters;

        public List<ParameterDeclaration> Parameters
            => _Parameters ?? (_Parameters = new List<ParameterDeclaration>());
    }
}