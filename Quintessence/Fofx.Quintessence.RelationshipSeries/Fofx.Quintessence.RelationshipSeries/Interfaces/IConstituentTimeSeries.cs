using System;
using System.Collections.Generic;

namespace Fofx
{
    /// <summary>
    /// Interface for constituent time series
    /// </summary>
    public interface IConstituentTimeSeries : ITimeSeries
    {
        void AddValue(DateTime date, object value);

        IEnumerable<IEntityDescriptor> GetConstituents(DateTime date);

        IEnumerable<IEntityDescriptor> GetConstituents();

        IEnumerable<IEntityDescriptor> GetConstituents(DateTime startDate, DateTime endDate);
    }
}
