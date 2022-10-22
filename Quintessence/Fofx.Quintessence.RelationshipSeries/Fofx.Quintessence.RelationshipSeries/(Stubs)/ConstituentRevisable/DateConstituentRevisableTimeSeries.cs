using System;

namespace Fofx
{
    [Serializable]
    internal sealed class DateConstituentRevisableTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentDatePoint, DateTime?>
    {
        internal DateConstituentRevisableTimeSeries()
        {
        }

        internal DateConstituentRevisableTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}