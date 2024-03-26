namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableCollectionAdapterFactory
{
    IImmutableCollectionAdapter<T> Create<T>();
    IImmutableCollectionAdapter<T> Create<T>(params T[] items);
    IImmutableCollectionAdapter<T>? GetDefaultValue<T>();
    IImmutableCollectionAdapter<T> Cast<T>(IEnumerable<T> collection);

    /// <summary>
    /// Creates a mutable collection with the same collection semantics as the tested type.
    /// </summary>
    /// <param name="items"></param>
    /// <returns></returns>
    ICollection<T> CreateMutable<T>(params T[] items);
}
