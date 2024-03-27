using System.Collections;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public abstract class ExplicitCollectionAdapter<T, TCollection>(TCollection collection) : ImmutableCollectionAdapter<T, TCollection>(collection), IReadOnlyCollection<T>
    where TCollection : IReadOnlyCollection<T>
{
    int IReadOnlyCollection<T>.Count => Collection.Count;
    IEnumerator<T> IEnumerable<T>.GetEnumerator() => Collection.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Collection).GetEnumerator();

}