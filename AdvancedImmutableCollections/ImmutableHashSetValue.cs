namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableHashSet{T}"/> to enable value semantics. It includes also factory methods for <see cref="ImmutableHashSetValue<T>"/>.
/// </summary>
public static class ImmutableHashSetValue
{
    public static ImmutableHashSetValue<T> WithValueSemantics<T>(this ImmutableHashSet<T> value)
        => new ImmutableHashSetValue<T>(value);

    public static ImmutableHashSetValue<T> Create<T>(ImmutableArray<T> source, IEqualityComparer<T>? equalityComparer)
    {
        var set = source.ToImmutableHashSet(equalityComparer);
        return new ImmutableHashSetValue<T>(set);
    }
}