using System;

namespace Fofx
{
    [Serializable]
    internal sealed class ConstituentDateArrayPoint : ConstituentDataPoint<DateTime[]>
    {
        public ConstituentDateArrayPoint(DateTime declarationDate, DateTime valueDate, NonKeyedAttributeSet nonKeyedAttributeSet)
        {
        }
    }
}
