namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public interface IImmutableListAdapterFactory : IImmutableCollectionAdapterFactory
{
    new IImmutableListAdapter<T> Create<T>();
    new IImmutableListAdapter<T> Create<T>(params T[] items);
    new IImmutableListAdapter<T>? GetDefaultValue<T>();
    new IImmutableListAdapter<T> Cast<T>(IEnumerable<T> collection);
}
