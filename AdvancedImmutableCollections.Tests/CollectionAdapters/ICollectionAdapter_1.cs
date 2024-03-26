namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface ICollectionAdapter<T> : ICollectionAdapter
{
    new IEnumerable<T> Collection { get; }
}
