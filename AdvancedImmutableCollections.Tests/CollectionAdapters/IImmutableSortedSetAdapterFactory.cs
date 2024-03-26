namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableSortedSetAdapterFactory : IImmutableSetAdapterFactory
{
    IImmutableSetAdapter<T> Create<T>(IComparer<T>? comparer);
    IImmutableSetAdapter<T> Create<T>(IComparer<T>? comparer, params T[] items);
}
