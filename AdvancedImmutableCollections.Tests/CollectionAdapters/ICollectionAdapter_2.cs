using System.Collections;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface ICollectionAdapter<T, out TCollection> : ICollectionAdapter<T>
    where TCollection : IReadOnlyCollection<T>
{
    new TCollection Collection { get; }

#if NETCOREAPP
    IEnumerable ICollectionAdapter.Collection => Collection;
    IEnumerable<T> ICollectionAdapter<T>.Collection => Collection;
#endif
}
