using System;

namespace Fofx
{
    [Serializable]
    internal sealed class DateConstituentArrayTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentDateArrayPoint, DateTime[]>
    {
        public DateConstituentArrayTimeSeries()
        {
        }

        public DateConstituentArrayTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}