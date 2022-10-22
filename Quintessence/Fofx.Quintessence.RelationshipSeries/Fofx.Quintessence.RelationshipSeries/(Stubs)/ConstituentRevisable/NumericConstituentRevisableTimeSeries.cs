using System;

namespace Fofx
{
    [Serializable]
    internal sealed class NumericConstituentRevisableTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentNumericPoint, double?>
    {
        internal NumericConstituentRevisableTimeSeries()
        {
        }

        internal NumericConstituentRevisableTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}