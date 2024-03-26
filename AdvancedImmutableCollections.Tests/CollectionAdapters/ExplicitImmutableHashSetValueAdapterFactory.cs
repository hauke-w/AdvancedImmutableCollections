using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ExplicitImmutableHashSetValueAdapterFactory : ImmutableHashSetAdapterFactory
{
    protected override IImmutableSetAdapter<T> GetDefault<T>() => new ExplicitIImmutableSetAdapter<T>(default(ImmutableHashSetValue<T>));
    protected override IImmutableSetAdapter<T> Cast<T>(IEnumerable<T> collection) => new ExplicitIImmutableSetAdapter<T>((ImmutableHashSetValue<T>)collection);
    protected override IImmutableSetAdapter<T> Create<T>(params T[] items) => new ExplicitIImmutableSetAdapter<T>(ImmutableHashSetValue.Create(items));
    protected override IImmutableSetAdapter<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] items) => new ExplicitIImmutableSetAdapter<T>(ImmutableHashSetValue.Create(equalityComparer, items));
    protected override IImmutableSetAdapter<T> Create<T>(IEnumerable<T> items) => new ExplicitIImmutableSetAdapter<T>(items.ToImmutableHashSet());
}
