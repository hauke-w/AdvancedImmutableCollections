using System.Collections;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public abstract class ImmutableCollectionAdapter<T, TCollection>(TCollection collection) : ICollectionAdapter<T, TCollection>
    where TCollection : IReadOnlyCollection<T>
{
    public TCollection Collection { get; } = collection;

#if !NETCOREAPP
    IEnumerable ICollectionAdapter.Collection => Collection;
    IEnumerable<T> ICollectionAdapter<T>.Collection => Collection;
#endif

    public override bool Equals(object? obj)
    {
        return obj is ICollectionAdapter otherAdapter
            ? Collection.Equals(otherAdapter.Collection)
            : Collection.Equals(obj);
    }

    public override int GetHashCode() => Collection.GetHashCode();
}
