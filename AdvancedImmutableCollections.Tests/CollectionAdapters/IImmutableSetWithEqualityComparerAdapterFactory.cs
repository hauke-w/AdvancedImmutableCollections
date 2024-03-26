namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableSetWithEqualityComparerAdapterFactory : IImmutableSetAdapterFactory
{
    IImmutableSetAdapter<T> Create<T>(IEqualityComparer<T>? equalityComparer);
    IImmutableSetAdapter<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items);
}
