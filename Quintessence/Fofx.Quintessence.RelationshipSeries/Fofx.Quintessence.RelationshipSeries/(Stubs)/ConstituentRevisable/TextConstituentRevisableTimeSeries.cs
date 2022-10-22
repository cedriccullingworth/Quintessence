using System;

namespace Fofx
{
    [Serializable]
    internal sealed class TextConstituentRevisableTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentTextPoint, string>
    {
        internal TextConstituentRevisableTimeSeries()
        {
        }

        internal TextConstituentRevisableTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}