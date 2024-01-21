using System.Diagnostics;

namespace AdvancedImmutableCollections;

/// <summary>
/// Immutable hash set with value semantics.
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay($$"""{{{nameof(GetDebuggerDipslay)}}(),nq}""")]
public readonly struct ImmutableHashSetValue<T> : IImmutableSet<T>, IEquatable<ImmutableHashSetValue<T>>
{
    private readonly ImmutableHashSet<T>? _Value;

    public ImmutableHashSetValue(ImmutableHashSet<T> set)
    {
        _Value = set ?? throw new ArgumentNullException(nameof(set));
    }

    public ImmutableHashSetValue(IEnumerable<T> items)
    {
        // important: use the comparer of items if it has one!
        _Value = items switch
        {
            null => throw new ArgumentNullException(nameof(items)),
            ImmutableHashSet<T> set => set,
            HashSet<T> set => set.ToImmutableHashSet(set.Comparer),
            _ => ImmutableHashSet.CreateRange(items),
        };
    }

    public ImmutableHashSetValue(IEqualityComparer<T> equalityComparer)
    {
        _Value = ImmutableHashSet<T>.Empty.WithComparer(equalityComparer);
    }

    public ImmutableHashSet<T> Value => _Value ?? ImmutableHashSet<T>.Empty;

    [MemberNotNullWhen(false, nameof(_Value))]
    public bool IsDefault => _Value is null;

    [MemberNotNullWhen(false, nameof(_Value))]
    public bool IsDefaultOrEmpty => _Value is null or { Count: 0 };

    public bool Contains(T item) => _Value is not null && _Value.Contains(item);

    public int Count => _Value is null ? 0 : _Value.Count;

    public ImmutableHashSet<T>.Enumerator GetEnumerator() => Value.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    public override bool Equals(object? obj) => obj is ImmutableHashSetValue<T> other && Equals(other);

    public bool Equals(ImmutableHashSetValue<T> other)
    {
        return _Value is { Count: > 0 }
            ? other._Value is { Count: > 0 } && _Value.SetEquals(other._Value)
            : other.Count == 0;
    }

    public static bool operator ==(ImmutableHashSetValue<T> left, ImmutableHashSetValue<T> right)
    {
        var a = left._Value;
        var b = right._Value;
        switch (a, b)
        {
            case (null, null):
            case (not null, not null) when ReferenceEquals(a, b):
                return true;
            case (not null, not null) when a.KeyComparer.Equals(b.KeyComparer) && a.Count == b.Count:
                return a.Count == 0 || a.SetEquals(b);
            default:
                return false;
        }        
    }

    public static bool operator !=(ImmutableHashSetValue<T> left, ImmutableHashSetValue<T> right) => !(left == right);

    public override int GetHashCode()
    {
        if (_Value is null)
        {
            return 0;
        }

        unchecked
        {
            int hash = 78137 * _Value.Count;
            foreach (var item in _Value)
            {
                if (item is not null)
                {
                    hash += item.GetHashCode();
                }
            }
            return hash;
        }
    }

    public static implicit operator ImmutableHashSet<T>(ImmutableHashSetValue<T> value) => value.Value;
    public static implicit operator ImmutableHashSetValue<T>(ImmutableHashSet<T> value) => new ImmutableHashSetValue<T>(value);

    public ImmutableHashSetValue<T> WithComparer(IEqualityComparer<T>? comparer)
    {
        if (_Value is null)
        {
            return comparer is null
                ? this
                : ImmutableHashSet<T>.Empty.WithComparer(comparer).WithValueSemantics();
        }

        comparer ??= EqualityComparer<T>.Default;
        return _Value.KeyComparer == comparer
            ? this
            : _Value.WithComparer(comparer).WithValueSemantics();
    }

    #region mutation operations
    /// <summary>
    /// Creates a new set containing all items of this set and the specified <paramref name="item"/>
    /// </summary>
    /// <param name="item">The element to add</param>
    /// <returns>A set value with the element added</returns>
    public ImmutableHashSetValue<T> Add(T item) => Value.Add(item).WithValueSemantics();

    /// <summary>
    /// Creates a new set containing all items of this set except the specified <paramref name="item"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns>A set value with the element removed</returns>
    public ImmutableHashSetValue<T> Remove(T item) => _Value is not { Count: > 0 } ? this : _Value.Remove(item).WithValueSemantics();

    /// <summary>
    /// Creates a new set containing all items of this set except the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items"></param>
    /// <returns>A set value with all items removed</returns>
    public ImmutableHashSetValue<T> Except(IEnumerable<T> items) => _Value is not { Count: > 0 } ? this : _Value.Except(items).WithValueSemantics();

    /// <summary>
    /// Returns an empty set value.
    /// </summary>
    /// <returns></returns>
    public ImmutableHashSetValue<T> Clear() => _Value is not { Count: > 0 } ? this : new ImmutableHashSetValue<T>(ImmutableHashSet.Create(_Value.KeyComparer));

    /// <summary>
    /// Creates a new set value containing only elements that are are present either in the current set or in the specified <paramref name="other"/> collection, but not both.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public ImmutableHashSetValue<T> SymmetricExcept(IEnumerable<T> other)
    {
        if (_Value is not { Count: > 0 })
        {
            // take all items from the other collection.
            // if _Value is not null the comparer of _Value will be applied to the result!

            switch (other)
            {
                case IReadOnlyCollection<T> { Count: 0 }:
                case ICollection<T> { Count: 0 }:
                case System.Collections.ICollection { Count: 0 }:
                    return this;
                case ImmutableHashSetValue<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                case ImmutableHashSet<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                default:
                    return other.ToImmutableHashSet(_Value?.KeyComparer);
            }
        }
        else
        {
           return _Value.SymmetricExcept(other);
        }
    }

    /// <summary>
    /// Creates a new set value containing only elements that are present in both the current set and the specified <paramref name="other"/> collection.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public ImmutableHashSetValue<T> Intersect(IEnumerable<T> other) => _Value is not { Count: > 0 } ? this : _Value.Intersect(other).WithValueSemantics();

    /// <summary>
    /// Creates a new set value containing all elements that are present in the current set or in the specified <paramref name="other"/> collection.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public ImmutableHashSetValue<T> Union(IEnumerable<T> other)
    {
        if (_Value is not { Count: > 0 })
        {
            switch (other)
            {
                case IReadOnlyCollection<T> { Count: 0 }:
                case ICollection<T> { Count: 0 }:
                case System.Collections.ICollection { Count: 0 }:
                    return this;
                case ImmutableHashSetValue<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                case ImmutableHashSet<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                default:
                    return other.ToImmutableHashSet(_Value?.KeyComparer);
            }
        }
        else
        {
            return Value.Union(other).WithValueSemantics();
        }
    }
    #endregion

    #region IImmutableSet
    IImmutableSet<T> IImmutableSet<T>.Add(T value) => Add(value);
    IImmutableSet<T> IImmutableSet<T>.Clear() => Clear();
    IImmutableSet<T> IImmutableSet<T>.Except(IEnumerable<T> other) => Except(other);
    IImmutableSet<T> IImmutableSet<T>.Intersect(IEnumerable<T> other) => Intersect(other);
    public bool IsProperSubsetOf(IEnumerable<T> other)
    {
        if (_Value is null)
        {
            return other switch
            {
                IReadOnlyCollection<T> collection => collection.Count != 0,
                ICollection<T> collection => collection.Count != 0,
                _ => other.GetEnumerator().MoveNext(),
            };
        }
        return _Value.IsProperSubsetOf(other);
    }

    public bool IsProperSupersetOf(IEnumerable<T> other) => _Value is not null && _Value.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => _Value is null || _Value.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other)
    {
        return _Value is null
            ? IsEmpty(other)
            : _Value.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other) => _Value is not null && _Value.Overlaps(other);

    IImmutableSet<T> IImmutableSet<T>.Remove(T value) => Remove(value);
    public bool SetEquals(IEnumerable<T> other)
    {
        return _Value is null
            ? IsEmpty(other)
            : _Value.SetEquals(other);
    }

    private static bool IsEmpty(IEnumerable<T> other)
    {
        return other switch
        {
            IReadOnlyCollection<T> collection => collection.Count == 0,
            ICollection<T> collection => collection.Count == 0,
            _ => !other.GetEnumerator().MoveNext(),
        };
    }

    IImmutableSet<T> IImmutableSet<T>.SymmetricExcept(IEnumerable<T> other) => SymmetricExcept(other);

    public bool TryGetValue(T equalValue, out T actualValue)
    {
        if (_Value is not { Count: > 0 })
        {
            actualValue = equalValue;
            return false;
        }
        return _Value.TryGetValue(equalValue, out actualValue);
    }

    IImmutableSet<T> IImmutableSet<T>.Union(IEnumerable<T> other) => Union(other);
    #endregion

    private string GetDebuggerDipslay()
    {
        switch (_Value)
        {
            case null:
            case { Count: 0 }:
                return $"ImmutableHashSetValue<{typeof(T).Name}> Count=0 Value=[]";
            case { Count: 1 }:
                return $"ImmutableHashSetValue<{typeof(T).Name}> Count=1 Value=[{_Value.First()}]";
            default:
                break;
        }

        var sb = new StringBuilder();
        sb.Append($"ImmutableHashSetValue<{typeof(T).Name}> Count={_Value.Count} Value=[");
        var enumerator = _Value.GetEnumerator();
        enumerator.MoveNext();
        sb.Append(enumerator.Current);

        int count = 1;
        while (sb.Length < 100 && enumerator.MoveNext())
        {
            count++;
            sb.Append(", ");
            sb.Append(enumerator.Current);
        }
        if (count < _Value.Count)
        {
            sb.Append(", ...");
        }
        return sb.ToString();
    }
}
