using System.Diagnostics;

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
    {
        var keyComparer = first.KeyComparer;
        var valueComparer = first.ValueComparer;
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
    {
        keyComparer ??= EqualityComparer<TKey>.Default;
        valueComparer ??= EqualityComparer<TValue>.Default;

        if (!HasComparers(first, keyComparer, valueComparer))
        {
            if (!HasComparers(other, keyComparer, valueComparer))
            {
                // this is the most complicated case: comparers of both dictionaries must be changed
                first = WithComparers(first, keyComparer, valueComparer, out var expectedCollisions);

                if (expectedCollisions is null)
                {
                    // by iterating over other we do not have to convert it to a dictionary with desired comparers
                    var uniqueKeysInOther = new HashSet<TKey>(keyComparer);
                    foreach (var (otherKey, otherValue) in other)
                    {
                        if (!first.TryGetValue(otherKey, out var value)
                            || !valueComparer.Equals(value, otherValue))
                        {
                            return false;
                        }
                        uniqueKeysInOther.Add(otherKey);
                    }
                    return uniqueKeysInOther.Count == first.Count;
                }
                else
                {
                    // we need a copy of expectedCollisions because have to remove processed values and at the same time keep all expected collisions
                    //var remainingCollisions = expectedCollisions.ToDictionary(kvp => kvp.Key, kvp => new HashSet<TValue>(kvp.Value, valueComparer));
                    var remainingCollisions = new Dictionary<TKey, HashSet<TValue>>(expectedCollisions.Count, keyComparer);
                    foreach (var (key, values) in expectedCollisions)
                    {
                        var remainingValues = new HashSet<TValue>(values, valueComparer);
                        Debug.Assert(remainingValues.Comparer == valueComparer);
                        remainingCollisions.Add(key, remainingValues);
                    }

                    // by iterating over other we do not have to convert it to a dictionary with desired comparers
                    foreach (var (otherKey, otherValue) in other)
                    {
                        if (expectedCollisions.TryGetValue(otherKey, out var expectedCollidingValues))
                        {
                            if (!expectedCollidingValues.Contains(otherValue))
                            {
                                // otherValue does not occur in first dictionary but we expect a collision.
                                // Therefore, both dictionaries are considered not equal
                                return false;
                            }
                            // else: collision in both dictionaries as expected
                            Debug.Assert(remainingCollisions[otherKey].Comparer == valueComparer);
                            remainingCollisions[otherKey].Remove(otherValue);
                        }
                        else if (!first.TryGetValue(otherKey, out var value)
                            || !valueComparer.Equals(value, otherValue))
                        {
                            return false;
                        }
                    }

                    // check remaining collisions is empty
                    foreach (var values in remainingCollisions.Values)
                    {
                        if (values.Count != 0)
                        {
                            // expected collision does not occur in other dictionary
                            return false;
                        }
                    }
                }
                return true;
            }
            else
            {
                // switch them
                var temp = other;
                other = first;
                first = temp;
            }
        }

        foreach (var (key, value) in first)
        {
            if (!other.TryGetValue(key, out var otherValue)
                || !valueComparer.Equals(value, otherValue))
            {
                return false;
            }
        }

        return true;
    }

    private static bool HasComparers<TKey, TValue>(in ImmutableDictionary<TKey, TValue> dictionary, in IEqualityComparer<TKey> keyComparer, in IEqualityComparer<TValue> valueComparer)
        where TKey : notnull
    {
        return dictionary.KeyComparer.Equals(keyComparer)
            && dictionary.ValueComparer.Equals(valueComparer);
    }

    private static ImmutableDictionary<TKey, TValue> WithComparers<TKey, TValue>(
        in ImmutableDictionary<TKey, TValue> dictionary,
        in IEqualityComparer<TKey> keyComparer,
        in IEqualityComparer<TValue> valueComparer,
        out Dictionary<TKey, HashSet<TValue>>? collisions)
        where TKey : notnull
    {
        Debug.Assert(keyComparer is not null);
        Debug.Assert(valueComparer is not null);
        collisions = null;

        var builder = ImmutableDictionary.CreateBuilder(keyComparer, valueComparer);
        foreach (var (key, value) in dictionary)
        {
            //
            // is there a collision? If yes, remove the item but add it to the collisions dictionary
            if (builder.TryGetValue(key, out var existingValue))
            {
                if (!valueComparer.Equals(existingValue, value))
                {
                    // there is a collision
                    builder.Remove(key);
                    collisions ??= new Dictionary<TKey, HashSet<TValue>>(keyComparer);
                    if (!collisions.TryGetValue(key, out var values))
                    {
                        values = new HashSet<TValue>(valueComparer);
                        collisions.Add(key, values);
                    }
                    values.Add(existingValue);
                    values.Add(value);
                }
            }
            else if (collisions is not null && collisions.TryGetValue(key, out var values))
            {
                values.Add(value);
            }
            else
            {
                builder.Add(key, value);
            }
        }
        return builder.ToImmutable();
    }
}
