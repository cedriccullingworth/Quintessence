using System;

namespace Fofx
{
    [Serializable]
    internal sealed class DataTypeConstituentRevisableTimeSeries : ConstituentRevisableTimeSeriesGeneric<ConstituentDataTypePoint, DataTypeValues>
    {
        public DataTypeConstituentRevisableTimeSeries()
        {
        }

        public DataTypeConstituentRevisableTimeSeries(DataManipulation.Interpolation interpolation, DataManipulation.Extrapolation extrapolation)
        {
        }
    }
}
