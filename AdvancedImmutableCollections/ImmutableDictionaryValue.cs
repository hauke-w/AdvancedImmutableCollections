namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableDictionary{TKey, TValue}"/> to enable value semantics.
/// </summary>
public static class ImmutableDictionaryValue
{
    public static ImmutableDictionaryValue<TKey, TValue> WithValueSemantics<TKey, TValue>(this ImmutableDictionary<TKey, TValue> value)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value);

#if NET8_0_OR_GREATER
    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(ReadOnlySpan<KeyValuePair<TKey, TValue>> value) // this method is used as a collection builder
        where TKey : notnull
    {
        return value
            .ToArray() // there is no method for creating ImmutableDictionary<TKey, TValue> that accepts ReadOnlySpan<KeyValuePair<TKey, TValue>>
            .ToImmutableDictionary()
            .WithValueSemantics();
    }
#endif

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(params KeyValuePair<TKey, TValue>[] value)
        where TKey : notnull
    {
        return value
            .ToImmutableDictionary()
            .WithValueSemantics();
    }

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value.ToImmutableDictionary());

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> value,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value.ToImmutableDictionary(keyComparer, valueComparer));

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        => ImmutableDictionary.Create(keyComparer, valueComparer).WithValueSemantics();

    /// <summary>
    /// Determines whether two dictionary are equal sets of key-value-pairs.
    /// The <see cref="ImmutableDictionary{TKey, TValue}.KeyComparer"/> of <paramref name="first"/> is used for comparing keys and
    /// the <see cref="ImmutableDictionary{TKey, TValue}.ValueComparer"/> of <paramref name="first"/> is used for comparing values.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="first"></param>
    /// <param name="other"></param>
    /// <returns></returns>
    public static bool SetEquals<TKey, TValue>(this ImmutableDictionary<TKey, TValue> first, ImmutableDictionary<TKey, TValue> other)
        where TKey : notnull
        => SetEqualsCore(first, other, first.KeyComparer, first.ValueComparer);

    /// <summary>
    /// Determines whether two dictionary are equal sets of key-value-pairs using the specified comparers.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    /// <param name="first"></param>
    /// <param name="other"></param>
    /// <param name="keyComparer">Comparer for comparing keys</param>
    /// <param name="valueComparer">Comparer for comparing values</param>
    /// <returns></returns>
    public static bool SetEquals<TKey, TValue>(this ImmutableDictionary<TKey, TValue> first, ImmutableDictionary<TKey, TValue> other,
        IEqualityComparer<TKey>? keyComparer, IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        => SetEqualsCore(first, other, keyComparer ?? EqualityComparer<TKey>.Default, valueComparer ?? EqualityComparer<TValue>.Default);

    /// <summary>
    /// Determines whether two dictionary are equal sets of key-value-pairs using the specified comparers.
    /// </summary>
    internal static bool SetEqualsCore<TKey, TValue>(this ImmutableDictionary<TKey, TValue> first, ImmutableDictionary<TKey, TValue> other,
        IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        where TKey : notnull
    {
        var otherWithComparers = new Dictionary<TKey, TValue>(other.Count, keyComparer);
        foreach (var (otherKey, otherValue) in other)
        {
            if (!first.TryGetKey(otherKey, out var actualFirstKey)
                || !keyComparer.Equals(actualFirstKey, otherKey)
                || !valueComparer.Equals(first[actualFirstKey], otherValue))
            {
                return false;
            }

            otherWithComparers.TryAdd(otherKey, otherValue);
        }

        return first.Count == otherWithComparers.Count;
    }
}
