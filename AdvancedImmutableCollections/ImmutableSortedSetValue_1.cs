using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections;

/// <summary>
/// Immutable sorted set with value semantics.
/// </summary>
/// <typeparam name="T"></typeparam>
[DebuggerDisplay($$"""{{{nameof(GetDebuggerDipslay)}}(),nq}""")]
#if NET8_0_OR_GREATER
[CollectionBuilder(typeof(ImmutableSortedSetValue), nameof(ImmutableSortedSetValue.Create))]
#endif
public readonly struct ImmutableSortedSetValue<T> : IImmutableSet<T>, IEquatable<ImmutableSortedSetValue<T>>
{
    private readonly ImmutableSortedSet<T>? _Value;

    public ImmutableSortedSetValue(ImmutableSortedSet<T> set)
    {
        _Value = set ?? throw new ArgumentNullException(nameof(set));
    }

    public ImmutableSortedSetValue(IEnumerable<T> items)
    {
        // important: use the comparer of items if it has one!
        _Value = items switch
        {
            null => throw new ArgumentNullException(nameof(items)),
            ImmutableSortedSet<T> set => set,
            SortedSet<T> set => set.ToImmutableSortedSet(set.Comparer),
            _ => ImmutableSortedSet.CreateRange(items),
        };
    }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="comparer"/> and <paramref name="items"/>.
    /// </summary>
    /// <param name="comparer">
    /// The comparer that will be used to compare elements of the set.
    /// If <see langword="null"/>, <see cref="Comparer{T}.Default"/> will be used.
    /// </param>
    /// <param name="items">The initial elements of the set</param>
    public ImmutableSortedSetValue(IComparer<T>? comparer, IEnumerable<T> items)
    {
        // important: use the comparer of items if it has one!
        _Value = items.ToImmutableSortedSet(comparer);
    }

    /// <summary>
    /// Initialized a new empty set value with the specified <paramref name="comparer"/>.
    /// </summary>
    /// <param name="comparer">
    /// The equality comparer that will be used to compare elements of the set.
    /// If <see langword="null"/>, <see cref="Comparer{T}.Default"/> will be used.
    /// </param>
    public ImmutableSortedSetValue(IComparer<T>? comparer)
    {
        _Value = ImmutableSortedSet<T>.Empty.WithComparer(comparer);
    }

    public ImmutableSortedSet<T> Value => _Value ?? ImmutableSortedSet<T>.Empty;

    [MemberNotNullWhen(false, nameof(_Value))]
    public bool IsDefault => _Value is null;

    [MemberNotNullWhen(false, nameof(_Value))]
    public bool IsDefaultOrEmpty => _Value is null or { Count: 0 };

    public bool Contains(T item) => _Value is not null && _Value.Contains(item);

    public int Count => _Value is null ? 0 : _Value.Count;

    public ImmutableSortedSet<T>.Enumerator GetEnumerator() => Value.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    public override bool Equals(object? obj) => obj is ImmutableSortedSetValue<T> other && Equals(other);

    /// <summary>
    /// Evaluates whether two sets are equivalent.
    /// </summary>
    /// <remarks>
    /// In contrast to the <see cref="ImmutableSortedSetValue{T}.operator=="/> the comparer of the underlying sets are not compared. The sets are equal if they contain the same items.
    /// </remarks>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(ImmutableSortedSetValue<T> other)
    {
        if (_Value is null)
        {
            return other._Value is null or { Count: 0 };
        }
        else if (other._Value is null)
        {
            return _Value.Count == 0;
        }

        return ImmutableSortedSetValue.SetEquals(_Value, other._Value);
    }

    public bool SetEquals(ImmutableSortedSet<T> otherValue)
    {
        if (_Value is null)
        {
            return otherValue.Count == 0;
        }

        return ImmutableSortedSetValue.SetEquals(_Value, otherValue);
    }

    public override int GetHashCode() => _Value is null ? 0 : _Value.GetSequenceHashCode();

    public static implicit operator ImmutableSortedSet<T>(ImmutableSortedSetValue<T> value) => value.Value;
    public static implicit operator ImmutableSortedSetValue<T>(ImmutableSortedSet<T> value) => new ImmutableSortedSetValue<T>(value);

    public ImmutableSortedSetValue<T> WithComparer(IComparer<T>? comparer)
    {
        if (_Value is null)
        {
            return comparer is null
                ? this
                : ImmutableSortedSet<T>.Empty.WithComparer(comparer).WithValueSemantics();
        }

        comparer ??= Comparer<T>.Default;
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
    public ImmutableSortedSetValue<T> Add(T item) => Value.Add(item).WithValueSemantics();

    /// <summary>
    /// Creates a new set containing all items of this set except the specified <paramref name="item"/>
    /// </summary>
    /// <param name="item"></param>
    /// <returns>A set value with the element removed</returns>
    public ImmutableSortedSetValue<T> Remove(T item) => _Value is not { Count: > 0 } ? this : _Value.Remove(item).WithValueSemantics();

    /// <summary>
    /// Creates a new set containing all items of this set except the specified <paramref name="items"/>.
    /// </summary>
    /// <param name="items"></param>
    /// <returns>A set value with all items removed</returns>
    public ImmutableSortedSetValue<T> Except(IEnumerable<T> items) => _Value is not { Count: > 0 } ? this : _Value.Except(items).WithValueSemantics();

    /// <summary>
    /// Returns an empty set value.
    /// </summary>
    /// <returns></returns>
    public ImmutableSortedSetValue<T> Clear() => _Value is not { Count: > 0 } ? this : new ImmutableSortedSetValue<T>(ImmutableSortedSet.Create(_Value.KeyComparer));

    /// <summary>
    /// Creates a new set value containing only elements that are are present either in the current set or in the specified <paramref name="other"/> collection, but not both.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public ImmutableSortedSetValue<T> SymmetricExcept(IEnumerable<T> other)
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
                case ImmutableSortedSetValue<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                case ImmutableSortedSet<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                default:
                    return other.ToImmutableSortedSet(_Value?.KeyComparer);
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
    public ImmutableSortedSetValue<T> Intersect(IEnumerable<T> other) => _Value is not { Count: > 0 } ? this : _Value.Intersect(other).WithValueSemantics();

    /// <summary>
    /// Creates a new set value containing all elements that are present in the current set or in the specified <paramref name="other"/> collection.
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public ImmutableSortedSetValue<T> Union(IEnumerable<T> other)
    {
        if (IsDefaultOrEmpty)
        {
            switch (other)
            {
                case IReadOnlyCollection<T> { Count: 0 }:
                case ICollection<T> { Count: 0 }:
                case System.Collections.ICollection { Count: 0 }:
                    return this;
                case ImmutableSortedSetValue<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                case ImmutableSortedSet<T> s:
                    return s.WithComparer(_Value?.KeyComparer);
                default:
                    return other.ToImmutableSortedSet(_Value?.KeyComparer);
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
        if (IsDefaultOrEmpty)
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
        return IsDefaultOrEmpty
            ? IsEmpty(other)
            : _Value.IsSupersetOf(other);
    }

    public bool Overlaps(IEnumerable<T> other) => _Value is not null && _Value.Overlaps(other);

    IImmutableSet<T> IImmutableSet<T>.Remove(T value) => Remove(value);
    public bool SetEquals(IEnumerable<T> other)
    {
        if (IsDefaultOrEmpty)
        {
            return IsEmpty(other);
        }
        return other switch
        {
            ImmutableSortedSetValue<T> otherValue => SetEquals(otherValue),
            ImmutableSortedSet<T> set => SetEquals(set),
            _ when _Value is null => IsEmpty(other),
            _ => _Value.SetEquals(other),
        };
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

    private string GetDebuggerDipslay() => CollectionValueDebuggerDisplay.GetDebuggerDisplay(_Value, nameof(ImmutableSortedSetValue<object>));
}
