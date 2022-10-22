using System;

namespace Fofx
{
    [Serializable]
    internal sealed class ConstituentDatePoint : ConstituentDataPoint<DateTime?>
    {
        internal ConstituentDatePoint(DateTime declarationDate, DateTime valueDate, NonKeyedAttributeSet nonKeyedAttributeSet)
        {
        }
    }
}
