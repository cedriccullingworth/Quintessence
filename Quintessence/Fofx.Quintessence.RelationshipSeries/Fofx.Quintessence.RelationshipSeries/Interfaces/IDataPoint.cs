using System;

namespace Fofx
{
    /// <summary>
    /// Interface for data points
    /// </summary>
    public interface IDataPoint
    {
        DateTime DeclarationDate { get; }
        DateTime ValueDate { get; }
        object Value { get; }
        IDataPoint Shift(DateTime valueDate, DateTime declarationDate);
        NonKeyedAttributeSet NonKeyedAttribute { get; }
    }
}
