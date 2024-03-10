namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableHashSet{T}"/> to enable value semantics. It includes also factory methods for <see cref="ImmutableHashSetValue{T}"/>.
/// </summary>
public static class ImmutableHashSetValue
{
    /// <summary>
    /// Creates a new <see cref="ImmutableHashSetValue{T}"/> from the specified <paramref name="set"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <returns></returns>
    public static ImmutableHashSetValue<T> WithValueSemantics<T>(this ImmutableHashSet<T> set)
        => new ImmutableHashSetValue<T>(set);

#if NET8_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="ImmutableHashSetValue{T}"/> from the specified <paramref name="items"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static ImmutableHashSetValue<T> Create<T>(ReadOnlySpan<T> items) // this method is used as a collection builder
        => ImmutableHashSet.Create(items).WithValueSemantics();
#endif

    /// <summary>
    /// Creates a new <see cref="ImmutableHashSetValue{T}"/> from the specified <paramref name="source"/> items and using the specified <paramref name="equalityComparer"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="equalityComparer"></param>
    /// <returns></returns>
    public static ImmutableHashSetValue<T> Create<T>(ImmutableArray<T> source, IEqualityComparer<T>? equalityComparer)
    {
        var set = source.ToImmutableHashSet(equalityComparer);
        return new ImmutableHashSetValue<T>(set);
    }

    /// <summary>
    /// Creates a new <see cref="ImmutableHashSetValue{T}"/> from the specified <paramref name="source"/> items and using the specified <paramref name="equalityComparer"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ImmutableHashSetValue<T> Create<T>(IEqualityComparer<T>? equalityComparer, params T[] source)
        => ImmutableHashSet.Create(equalityComparer, source).WithValueSemantics();

    /// <summary>
    /// Creates a new <see cref="ImmutableHashSetValue{T}"/> from the specified <paramref name="source"/> items
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <returns></returns>
    public static ImmutableHashSetValue<T> Create<T>(params T[] source)
        => ImmutableHashSet.Create(source).WithValueSemantics();

    public static ImmutableHashSetValue<T> Empty<T>()
        => ImmutableHashSet<T>.Empty.WithValueSemantics();
}