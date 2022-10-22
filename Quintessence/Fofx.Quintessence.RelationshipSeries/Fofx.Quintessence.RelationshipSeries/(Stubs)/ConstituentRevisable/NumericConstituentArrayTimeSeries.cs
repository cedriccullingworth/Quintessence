using System;

namespace Fofx
{
    [Serializable]
    internal sealed class NumericConstituentArrayTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentNumericArrayPoint, double[]>
    {
        public NumericConstituentArrayTimeSeries()
        {
        }

        public NumericConstituentArrayTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}