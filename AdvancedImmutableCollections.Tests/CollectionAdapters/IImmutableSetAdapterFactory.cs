namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableSetAdapterFactory : IImmutableCollectionAdapterFactory
{
    new IImmutableSetAdapter<T> Create<T>();
    new IImmutableSetAdapter<T> Create<T>(params T[] items);
    IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items);
    new IImmutableSetAdapter<T>? GetDefaultValue<T>();
    new IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection);
}
