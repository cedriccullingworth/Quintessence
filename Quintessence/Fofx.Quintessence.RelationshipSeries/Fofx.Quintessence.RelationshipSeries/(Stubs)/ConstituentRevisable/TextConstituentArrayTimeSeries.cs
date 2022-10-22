using System;

namespace Fofx
{
    [Serializable]
    internal sealed class TextConstituentArrayTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentTextArrayPoint, string[]>
    {
        public TextConstituentArrayTimeSeries()
        {
        }

        public TextConstituentArrayTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}