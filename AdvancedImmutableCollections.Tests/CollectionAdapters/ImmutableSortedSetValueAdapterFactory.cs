using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ImmutableSortedSetValueAdapterFactory : ImmutableSortedSetAdapterFactory
{
    protected override IImmutableSetAdapter<T> GetDefault<T>() => new ImmutableSortedSetValueAdapter<T>(default(ImmutableSortedSetValue<T>));
    protected override IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection) => new ImmutableSortedSetValueAdapter<T>((ImmutableSortedSetValue<T>)collection);
    protected override IImmutableSetAdapter<T> Create<T>(params T[] items) => new ImmutableSortedSetValueAdapter<T>(items);
    protected override IImmutableSetAdapter<T> Create<T>(IComparer<T>? comparer, params T[] items) => new ImmutableSortedSetValueAdapter<T>(comparer, items);
    protected override IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items) => new ImmutableSortedSetValueAdapter<T>(items.ToImmutableSortedSet());
}
