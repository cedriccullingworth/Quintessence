namespace Fofx
{
    /// <summary>
    /// Interface for revisable time series
    /// </summary>
    public interface ICharacteristicRevisableTimeSeries : IConstituentDataTimeSeries
    {
        void AddValue(IEntityDescriptor entityCode, object value, NonKeyedAttributeSet nonKeyedAttributeSet = null);
    }
}
