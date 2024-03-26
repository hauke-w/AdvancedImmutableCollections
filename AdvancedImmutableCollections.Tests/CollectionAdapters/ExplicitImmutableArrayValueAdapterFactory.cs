namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ExplicitImmutableArrayValueAdapterFactory : ImmutableListAdapterFactory
{
    protected override IImmutableListAdapter<T> GetDefault<T>() => new ExplicitIImmutableListAdapter<T>(default(ImmutableArrayValue<T>));
    protected override IImmutableListAdapter<T> Cast<T>(IEnumerable<T> collection) => new ExplicitIImmutableListAdapter<T>((ImmutableArrayValue<T>)collection);
    protected override IImmutableListAdapter<T> Create<T>(params T[] items) => new ExplicitIImmutableListAdapter<T>(ImmutableArrayValue.Create(items));
}
