using System;

namespace Fofx
{
    /// <summary>
    /// A class representing a characteristic date point
    /// </summary>
    [Serializable]
    internal sealed class CharacteristicDatePoint : ConstituentDataPoint<DateTime?>
    {
        public CharacteristicDatePoint(NonKeyedAttributeSet set) { }
    }
}
