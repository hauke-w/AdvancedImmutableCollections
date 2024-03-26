using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ImmutableHashSetValueAdapterFactory : ImmutableHashSetAdapterFactory
{
    protected override IImmutableSetAdapter<T> GetDefault<T>() => new ImmutableHashSetValueAdapter<T>(default(ImmutableHashSetValue<T>));
    protected override IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection) => new ImmutableHashSetValueAdapter<T>((ImmutableHashSetValue<T>)collection);
    protected override IImmutableSetAdapter<T> Create<T>(params T[] items) => new ImmutableHashSetValueAdapter<T>(items);
    protected override IImmutableSetAdapter<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items) => new ImmutableHashSetValueAdapter<T>(equalityComparer, items);
    protected override IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items) => new ImmutableHashSetValueAdapter<T>(items.ToImmutableHashSet());
}
