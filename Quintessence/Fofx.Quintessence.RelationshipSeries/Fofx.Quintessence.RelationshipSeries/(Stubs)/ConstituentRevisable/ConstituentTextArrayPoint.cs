using System;

namespace Fofx
{
    [Serializable]
    internal sealed class ConstituentTextArrayPoint : ConstituentDataPoint<string[]>
    {
        public ConstituentTextArrayPoint(DateTime declarationDate, DateTime valueDate, NonKeyedAttributeSet nonKeyedAttributeSet)
        {
        }
    }
}
