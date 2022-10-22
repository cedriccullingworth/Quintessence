using System;

namespace Fofx
{
    [Serializable]
    internal sealed class RelationshipTimeSeriesKey : TimeSeriesKey, IComparable<RelationshipTimeSeriesKey>
    {
        public RelationshipDescriptor Relationship { get; }

        public int CompareTo(RelationshipTimeSeriesKey other)
        {
            throw new NotImplementedException();
        }
    }
}
