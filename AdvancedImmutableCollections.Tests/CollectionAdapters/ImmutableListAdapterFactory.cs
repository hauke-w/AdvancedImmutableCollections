namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public abstract class ImmutableListAdapterFactory : IImmutableListAdapterFactory
{
    protected abstract IImmutableListAdapter<T> Create<T>(params T[] items);

    protected abstract IImmutableListAdapter<T> GetDefault<T>();
    protected abstract IImmutableListAdapter<T> Cast<T>(IEnumerable<T> collection);

    IImmutableListAdapter<T> IImmutableListAdapterFactory.Cast<T>(IEnumerable<T> collection) => Cast(collection);
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Cast<T>(IEnumerable<T> collection) => Cast(collection);
    IImmutableListAdapter<T> IImmutableListAdapterFactory.Create<T>() => Create<T>();
    IImmutableListAdapter<T> IImmutableListAdapterFactory.Create<T>(params T[] items) => Create(items);
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Create<T>() => Create<T>();
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Create<T>(params T[] items) => Create(items);
    ICollection<T> IImmutableCollectionAdapterFactory.CreateMutable<T>(params T[] items) => items.ToList();
    IImmutableListAdapter<T>? IImmutableListAdapterFactory.GetDefaultValue<T>() => GetDefault<T>();
    IImmutableCollectionAdapter<T>? IImmutableCollectionAdapterFactory.GetDefaultValue<T>() => GetDefault<T>();
}
