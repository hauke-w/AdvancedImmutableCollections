namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableCollectionAdapter<T> : IReadOnlyCollection<T>, ICollectionAdapter<T>
{
    IReadOnlyCollection<T> Add(T value);
    IReadOnlyCollection<T> AddRange(IEnumerable<T> items);
    IReadOnlyCollection<T> Remove(T value);
    IReadOnlyCollection<T> Clear();
}
