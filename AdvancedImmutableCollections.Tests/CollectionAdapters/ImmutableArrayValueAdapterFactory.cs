namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ImmutableArrayValueAdapterFactory : ImmutableListAdapterFactory
{
    protected override IImmutableListAdapter<T> GetDefault<T>() => new ImmutableArrayValueAdapter<T>(default(ImmutableArrayValue<T>));
    protected override IImmutableListAdapter<T> Cast<T>(IEnumerable<T> collection) => new ImmutableArrayValueAdapter<T>((ImmutableArrayValue<T>)collection);
    protected override IImmutableListAdapter<T> Create<T>(params T[] items) => new ImmutableArrayValueAdapter<T>(items);
}
