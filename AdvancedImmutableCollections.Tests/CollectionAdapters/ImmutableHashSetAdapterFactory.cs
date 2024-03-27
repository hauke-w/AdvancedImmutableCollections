using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public abstract class ImmutableHashSetAdapterFactory : IImmutableSetWithEqualityComparerAdapterFactory
{
    protected abstract IImmutableSetAdapter<T> Create<T>(params T[] items);
    protected abstract IImmutableSetAdapter<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items);
    protected abstract IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items);
    protected abstract IImmutableSetAdapter<T> GetDefault<T>();
    protected abstract IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection);

    IImmutableSetAdapter<T> IImmutableSetAdapterFactory.Create<T>() => Create<T>();
    IImmutableSetAdapter<T> IImmutableSetAdapterFactory.Create<T>(params T[] items) => Create(items);
    IImmutableSetAdapter<T> IImmutableSetWithEqualityComparerAdapterFactory.Create<T>(IEqualityComparer<T>? equalityComparer) => Create(equalityComparer);
    IImmutableSetAdapter<T> IImmutableSetWithEqualityComparerAdapterFactory.Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items) => Create(equalityComparer, items);
    IImmutableSetAdapter<T>? IImmutableSetAdapterFactory.GetDefaultValue<T>() => GetDefault<T>();
    IImmutableSetAdapter<T> IImmutableSetAdapterFactory.Cast<T>(IEnumerable<T> collection) => Cast(collection);
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Create<T>() => Create<T>();
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Create<T>(params T[] items) => Create(items);
    IImmutableCollectionAdapter<T>? IImmutableCollectionAdapterFactory.GetDefaultValue<T>() => GetDefault<T>();
    IImmutableCollectionAdapter<T> IImmutableCollectionAdapterFactory.Cast<T>(IEnumerable<T> collection) => Cast(collection);
    ICollection<T> IImmutableCollectionAdapterFactory.CreateMutable<T>(params T[] items) => new HashSet<T>(items);
    IImmutableSetAdapter<T> IImmutableSetAdapterFactory.Create<T>(IEnumerable<T> items) => Create(items);
}