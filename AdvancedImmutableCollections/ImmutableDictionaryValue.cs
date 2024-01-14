namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableDictionary{TKey, TValue}"/> to enable value semantics.
/// </summary>
public static class ImmutableDictionaryValue
{
    public static ImmutableDictionaryValue<TKey, TValue> WithValueSemantics<TKey, TValue>(this ImmutableDictionary<TKey, TValue> value)
        where TKey : notnull
        => new ImmutableDictionaryValue<TKey, TValue>(value);
}
