using System.IO;

namespace Shipwreck.VB6Models.Parsing
{
    public sealed class SourceFileReader
    {
        public SourceFileReader(string fileName)
        {
            FileName = Path.GetFullPath(fileName);
        }

        public string FileName { get; }

        public void Load()
        {
            using (var sr = new StreamReader(FileName))
            using (var tp = new TokenParser(sr))
            {
                while (tp.Read())
                {
                }
            }
        }
    }
}