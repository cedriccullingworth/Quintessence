using System;
using System.Collections.Generic;

namespace Fofx
{
    /// <summary>
    /// Interface for time series
    /// </summary>
    public interface ITimeSeries : IEnumerable<KeyValuePair<DateTime, IDataPoint>>
    {
        int Count { get; }

        IEnumerable<DateTime> Dates { get; }

        DateTime StartDate { get; }

        DateTime EndDate { get; }

        DateTime CachedStartDate { get; set; }

        DateTime CachedEndDate { get; set; }

        DataManipulation.Interpolation Interpolation { get; }

        DataManipulation.Extrapolation Extrapolation { get; }

        void Compact();

        int FindNextDate(DateTime lDate);

        int FindPreviousDate(DateTime lDate);

        DateTime GetNextDate(DateTime lDate);

        DateTime GetPreviousDate(DateTime lDate);

        DateTime GetDate(int index);

        IDataPoint GetValue(DateTime date);

        /// <summary>Try to get the value with out interpolation</summary>
        bool TryGetValue(DateTime date, out IDataPoint value);

        TimeSeriesType Type { get; }

        Type ElementType { get; }

        TimeSeriesDataType ValueType { get; }

        bool ContainsDates(IList<DateTime> dates);

        bool ContainsDate(DateTime key);

        ITimeSeries GetView(ITimeSeriesIterator iterator, DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation, NonKeyedAttributeSet nonKeyAttributes);

        bool TryMerge(ITimeSeries mergeMe);

        bool IsInRange(ITimeSeriesIterator iterator);
    }
}
