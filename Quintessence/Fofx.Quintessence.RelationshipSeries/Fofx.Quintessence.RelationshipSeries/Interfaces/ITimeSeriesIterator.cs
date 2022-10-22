using System;

namespace Fofx
{
    /// <summary>
    /// Interface for time series iterators
    /// </summary>
    public interface ITimeSeriesIterator : IComparable<ITimeSeriesIterator>
    {
        DateTime StartDate { get; }
        DateTime EndDate { get; }
        DateTime Iaad { get; }
        int StartOffset { get; }
        int EndOffset { get; }
        bool AllRevisions { get; }
        int Revision { get; }
    }
}
