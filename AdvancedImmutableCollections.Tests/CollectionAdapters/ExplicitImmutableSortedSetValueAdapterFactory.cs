using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ExplicitImmutableSortedSetValueAdapterFactory : ImmutableSortedSetAdapterFactory
{
    protected override IImmutableSetAdapter<T> GetDefault<T>() => new ExplicitIImmutableSetAdapter<T>(default(ImmutableSortedSetValue<T>));
    protected override IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection) => new ExplicitIImmutableSetAdapter<T>((ImmutableSortedSetValue<T>)collection);
    protected override IImmutableSetAdapter<T> Create<T>(params T[] items) => new ExplicitIImmutableSetAdapter<T>(ImmutableSortedSetValue.Create(items));
    protected override IImmutableSetAdapter<T> Create<T>(IComparer<T>? comparer, params T[] items) => new ExplicitIImmutableSetAdapter<T>(ImmutableSortedSetValue.Create(comparer, items));
    protected override IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items) => new ExplicitIImmutableSetAdapter<T>(items.ToImmutableSortedSet());
}
