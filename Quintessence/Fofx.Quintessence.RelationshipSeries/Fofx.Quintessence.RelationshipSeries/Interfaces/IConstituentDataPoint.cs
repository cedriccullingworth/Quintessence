using System.Collections.Generic;

namespace Fofx
{
    /// <summary>
    /// Interface for constituent data points
    /// </summary>
    public interface IConstituentDataPoint : IDataPoint, IEnumerable<KeyValuePair<IEntityDescriptor, object>>
    {
        object GetValue(IEntityDescriptor child);

        NonKeyedAttributeSet GetNonKey(IEntityDescriptor child);

        void Add(IEntityDescriptor child, object obj, NonKeyedAttributeSet nonKey);

        void AddWithNull(IEntityDescriptor child, object obj);

        IList<IEntityDescriptor> Entities { get; }
    }
}