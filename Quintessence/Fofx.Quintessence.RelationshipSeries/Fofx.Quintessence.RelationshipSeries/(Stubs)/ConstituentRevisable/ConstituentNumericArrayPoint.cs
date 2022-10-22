using System;

namespace Fofx
{
    [Serializable]
    internal sealed class ConstituentNumericArrayPoint : ConstituentDataPoint<double[]>
    {
        public ConstituentNumericArrayPoint(DateTime declarationDate, DateTime valueDate, NonKeyedAttributeSet nonKeyedAttributeSet)
        {
        }
    }
}
