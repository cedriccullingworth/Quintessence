using System;

namespace Fofx
{
    [Serializable]
    public sealed class RelationshipDescriptor : IComparable
    {
        public int ValueDefinition { get; set; }
        public SourceDescriptor Source { get; set; }

        public int CompareTo(object obj)
        {
            throw new NotImplementedException();
        }
    }
}