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

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(IEnumerable<KeyValuePair<TKey, TValue>> value)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value.ToImmutableDictionary());

    public static ImmutableDictionaryValue<TKey, TValue> Create<TKey, TValue>(
        this IEnumerable<KeyValuePair<TKey, TValue>> value,
        IEqualityComparer<TKey>? keyComparer,
        IEqualityComparer<TValue>? valueComparer)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value.ToImmutableDictionary(keyComparer, valueComparer));
}
