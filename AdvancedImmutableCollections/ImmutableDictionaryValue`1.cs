using System.Collections;

namespace AdvancedImmutableCollections;

/// <summary>
/// Immutable dictionary with value semantics.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
public readonly struct ImmutableDictionaryValue<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, IEquatable<ImmutableDictionaryValue<TKey, TValue>>
    where TKey : notnull
{
    // TODO: implement IImmutableDictionary<TKey, TValue>

    public ImmutableDictionaryValue(ImmutableDictionary<TKey, TValue> value)
    {
        _Value = value ?? throw new ArgumentNullException(nameof(value));
    }

    private readonly ImmutableDictionary<TKey, TValue>? _Value;

    public ImmutableDictionary<TKey, TValue> Value => _Value ?? ImmutableDictionary<TKey, TValue>.Empty;

    public TValue this[TKey key] => _Value is null ? throw new KeyNotFoundException() : _Value[key];

    public IEnumerable<TKey> Keys => _Value is null ? Enumerable.Empty<TKey>() : _Value.Keys;
    public IEnumerable<TValue> Values => _Value is null ? Enumerable.Empty<TValue>() : _Value.Values;
    public int Count => _Value is null ? 0 : _Value.Count;

    public bool IsDefault => _Value is null;
    public bool IsDefaultOrEmpty => _Value is null or { Count: 0 };

    public bool ContainsKey(TKey key) => _Value is { Count: > 0 } && _Value.ContainsKey(key);

#if !NETCOREAPP
#pragma warning disable CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
#endif
    public bool TryGetValue(TKey key, [MaybeNullWhen(false)] out TValue value)
    {
        if (_Value is null)
        {
            value = default;
            return false;
        }
        return _Value.TryGetValue(key, out value);
    }
#if !NETCOREAPP
#pragma warning restore CS8767 // Nullability of reference types in type of parameter doesn't match implicitly implemented member (possibly because of nullability attributes).
#endif

    public ImmutableDictionaryValue<TKey, TValue> Add(TKey key, TValue value) => Value.Add(key, value).WithValueSemantics();
    public ImmutableDictionaryValue<TKey, TValue> AddRange(IEnumerable<KeyValuePair<TKey, TValue>> pairs) => Value.AddRange(pairs).WithValueSemantics();

    public ImmutableDictionaryValue<TKey, TValue> Clear()
    {
        if (_Value is null or { Count: 0 })
        {
            return this;
        }
        return _Value.Clear().WithValueSemantics();
    }

    public ImmutableDictionaryValue<TKey, TValue> Remove(TKey key)
    {
        if (_Value is null or { Count: 0 })
        {
            return this;
        }
        return _Value.Remove(key).WithValueSemantics();
    }

    public ImmutableDictionaryValue<TKey, TValue> RemoveRange(IEnumerable<TKey> keys)
    {
        if (_Value is null or { Count: 0 })
        {
            return this;
        }
        return _Value.RemoveRange(keys).WithValueSemantics();
    }

    public ImmutableDictionaryValue<TKey, TValue> SetItem(TKey key, TValue value) => Value.SetItem(key, value).WithValueSemantics();

    public ImmutableDictionaryValue<TKey, TValue> SetItems(IEnumerable<KeyValuePair<TKey, TValue>> items) => Value.SetItems(items).WithValueSemantics();

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is ImmutableDictionaryValue<TKey, TValue> other && Equals(other);
    public bool Equals(ImmutableDictionaryValue<TKey, TValue> other)
    {
        var otherDict = other._Value;
        if (_Value is null)
        {
            return otherDict is null or { Count: 0 };
        }
        if (otherDict is null)
        {
            return Count == 0;
        }

        if (ReferenceEquals(_Value, otherDict))
        {
            return true;
        }

        if (_Value.Count != otherDict.Count
            || !_Value.KeyComparer.Equals(otherDict.KeyComparer)
            || !_Value.ValueComparer.Equals(otherDict.ValueComparer))
        {
            return false;
        }

        foreach (var (key, value) in _Value)
        {
            if (!otherDict.TryGetValue(key, out var otherValue)
                || !Equals(value, otherValue))
            {
                return false;
            }
        }
        return true;
    }

    public override int GetHashCode()
    {
        int hash = 0;
        if (_Value is not null)
        {
            unchecked
            {
                foreach (var (key, value) in _Value)
                {
                    var keyHash = key.GetHashCode() * 97967; // 97967 is prime
                    var valueHash = value is null ? 0 : value.GetHashCode() * 89237; // 89237 is prime

                    hash += keyHash ^ valueHash;
                }
            }
        }
        return hash;
    }

    public ImmutableDictionary<TKey, TValue>.Enumerator GetEnumerator() => Value.GetEnumerator();

    IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator() => _Value is null ? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator() : _Value.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _Value is null ? Enumerable.Empty<KeyValuePair<TKey, TValue>>().GetEnumerator() : _Value.GetEnumerator();

    public static implicit operator ImmutableDictionary<TKey, TValue>(ImmutableDictionaryValue<TKey, TValue> value) => value.Value;
    public static implicit operator ImmutableDictionaryValue<TKey, TValue>(ImmutableDictionary<TKey, TValue> value) => new ImmutableDictionaryValue<TKey, TValue>(value);
}
