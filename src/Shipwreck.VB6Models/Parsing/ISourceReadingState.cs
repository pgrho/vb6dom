using System.Collections.Generic;

namespace Shipwreck.VB6Models.Parsing
{
    internal interface ISourceReadingState
    {
        bool Accept(SourceFileReader reader, IReadOnlyList<Token> tokens);
    }
}