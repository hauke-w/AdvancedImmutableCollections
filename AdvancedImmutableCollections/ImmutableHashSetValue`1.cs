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

    public ImmutableHashSet<T> Value => _Value ?? ImmutableHashSet<T>.Empty;

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

    public override int GetHashCode()
    {
        if (_Value is null)
        {
            return 0;
        }

        int hash = 78137 * _Value.Count;
        unchecked
        {
            foreach (var item in _Value)
            {
                if (item is not null)
                {
                    hash += item.GetHashCode();
                }
            }
        }
        return hash;
    }

    public static implicit operator ImmutableHashSet<T>(ImmutableHashSetValue<T> value) => value.Value;
    public static implicit operator ImmutableHashSetValue<T>(ImmutableHashSet<T> value) => new ImmutableHashSetValue<T>(value);

    #region mutation operations
    public ImmutableHashSetValue<T> Add(T item) => Value.Add(item).WithValueSemantics();

    public ImmutableHashSetValue<T> AddRange(IEnumerable<T> items) => Value.Union(items).WithValueSemantics();

    public ImmutableHashSetValue<T> Remove(T item) => _Value is not { Count: > 0 } ? this : _Value.Remove(item).WithValueSemantics();

    public ImmutableHashSetValue<T> Except(IEnumerable<T> items) => _Value is not { Count: > 0 } ? this : _Value.Except(items).WithValueSemantics();

    public ImmutableHashSetValue<T> Clear() => _Value is not { Count: > 0 } ? this : new ImmutableHashSetValue<T>(ImmutableHashSet<T>.Empty);
    public ImmutableHashSetValue<T> SymmetricExcept(IEnumerable<T> other) => _Value is not { Count: > 0 } ? this : _Value.SymmetricExcept(other).WithValueSemantics();
    public ImmutableHashSetValue<T> Intersect(IEnumerable<T> other) => _Value is not { Count: > 0 } ? this : _Value.Intersect(other).WithValueSemantics();
    public ImmutableHashSetValue<T> Union(IEnumerable<T> other) => Value.Union(other).WithValueSemantics();
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
