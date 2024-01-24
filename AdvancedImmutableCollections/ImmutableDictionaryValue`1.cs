using System.Collections;
using System.Runtime.CompilerServices;

namespace AdvancedImmutableCollections;

/// <summary>
/// Immutable dictionary with value semantics.
/// </summary>
/// <typeparam name="TKey"></typeparam>
/// <typeparam name="TValue"></typeparam>
#if NET8_0_OR_GREATER
[CollectionBuilder(typeof(ImmutableDictionaryValue), nameof(ImmutableDictionaryValue.Create))]
#endif
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

    #region equality
    public override bool Equals([NotNullWhen(true)] object? obj)
    => obj is ImmutableDictionaryValue<TKey, TValue> other && Equals(other);

    /// <summary>
    /// Determines whether both dictionaries contain the same set of key-value-pairs (same semantics as set equals). This operation considers the key and value comparers.
    /// </summary>
    /// <remarks>
    /// This operation hase semantics of <c>this.ToHashSet(KeyValuePairComparer.Create(Value.KeyComparer, Value.ValueComparer)).SetEquals(other)</c>
    /// </remarks>
    /// <param name="other"></param>
    /// <returns></returns>
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

        if (_Value.Count != otherDict.Count)
        {
            if (_Value.KeyComparer.Equals(otherDict.KeyComparer)
                && _Value.ValueComparer.Equals(otherDict.ValueComparer))
            {
                return false;
            }
            // otherwise there is a chance that one of the dictionaries contains duplicate keys according to the other dictionary's key comparer
        }
        else if (_Value.Count == 0)
        {
            return true;
        }

        return _Value.SetEquals(otherDict);
    }

    /// <summary>
    /// Evaluates whether two set dictionary are interchangeably equal considering their elements and comparers.
    /// </summary>
    /// <remarks>
    /// The dictionaries are equal if they contain the same elements and their <see cref="Value">underlying dictionaries</see> have equal <see cref="ImmutableDictionary{TKey, TValue}.KeyComparer"/> and <see cref="ImmutableDictionary{TKey, TValue}.ValueComparer"/>.
    /// Keys and values are compared using the respective default comparer (<see cref="EqualityComparer{T}.Default"/>).
    /// 
    /// <para>
    /// The <see langword="default"/> value is interchangeable with an empty dictionary that has <see cref="EqualityComparer{T}.Default">default</see> key and value comparers.
    /// </para>
    /// </remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if
    /// <list type="bullet">
    /// <item>the <see cref="ImmutableDictionary{TKey, TValue}.KeyComparer"/> of the underlying dictionaries are equal <strong>and</strong></item>
    /// <item>the <see cref="ImmutableDictionary{TKey, TValue}.ValueComparer"/> of the underlying dictionaries are equal <strong>and</strong></item>
    /// <item>The dictionaries contain equal sets of key value pairs</item>
    /// </list>
    /// Otherwise <see langword="true"/>.
    /// </returns>
    public static bool operator ==(ImmutableDictionaryValue<TKey, TValue> left, ImmutableDictionaryValue<TKey, TValue> right)
    {
        var a = left._Value;
        var b = right._Value;
        switch (a, b)
        {
            case (null, null):
            case (not null, not null) when ReferenceEquals(a, b):
            case ({ Count: 0 }, null) when HasDefaultComparers(a):
            case (null, { Count: 0 }) when HasDefaultComparers(b):
                return true;
            case (not null, not null) when a.Count == b.Count && a.KeyComparer.Equals(b.KeyComparer) && a.ValueComparer.Equals(b.KeyComparer):
                return a.Count == 0 
                    || a.SetEqualsCore(b, EqualityComparer<TKey>.Default, EqualityComparer<TValue>.Default); // compare using default comparers!
            default:
                return false;
        }

        static bool HasDefaultComparers(ImmutableDictionary<TKey, TValue> d)
            => d.KeyComparer.Equals(EqualityComparer<TKey>.Default) && d.ValueComparer.Equals(EqualityComparer<TValue>.Default);
    }

    /// <summary>
    /// Evaluates whether two dictionary values are not interchangeably equal considering their elements and comparers.
    /// </summary>
    /// <remarks>
    /// This is the inverse of the <see cref="operator ==(ImmutableDictionaryValue{TKey, TValue}, ImmutableDictionaryValue{TKey, TValue})"/>
    /// </remarks>
    /// <param name="left"></param>
    /// <param name="right"></param>
    /// <returns>
    /// <see langword="true"/> if
    /// <list type="bullet">
    /// <item>the <see cref="ImmutableDictionary{TKey, TValue}.KeyComparer"/> of the underlying dictionaries are not equal <strong>or</strong></item>
    /// <item>the <see cref="ImmutableDictionary{TKey, TValue}.ValueComparer"/> of the underlying dictionaries are not equal <strong>or</strong></item>
    /// <item>The dictionaries contain different sets of key value pairs</item>
    /// </list>
    /// Otherwise <see langword="false"/>.
    /// </returns>
    public static bool operator !=(ImmutableDictionaryValue<TKey, TValue> left, ImmutableDictionaryValue<TKey, TValue> right) => !(left == right);
    #endregion

    public override int GetHashCode()
    {
        int hash = 0;
        if (_Value is not null)
        {
            var keyComparer = _Value.KeyComparer;
            var valueComparer = _Value.ValueComparer;
            unchecked
            {
                foreach (var (key, value) in _Value)
                {
                    var keyHash = keyComparer.GetHashCode(key) * 97967; // 97967 is prime
                    var valueHash = value is null
                        ? 0
                        : valueComparer.GetHashCode(value) * 89237; // 89237 is prime

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
