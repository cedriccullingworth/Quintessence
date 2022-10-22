namespace Fofx
{
    /// <summary>
    /// Interface for translators
    /// </summary>
    public interface ITranslator
    {
        NonKeyedAttributeSet GetNonKeyedAttributeSet(int id);

        bool TryGetEntityDescriptorByID(int entityID, out IEntityDescriptor result);
        bool TryGetEntityDescriptorByID(int entityID, Preference preference, out IEntityDescriptor result);
    }
}