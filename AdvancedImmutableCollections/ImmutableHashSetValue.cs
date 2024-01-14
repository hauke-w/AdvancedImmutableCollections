using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading;

namespace AdvancedImmutableCollections;
public static class ImmutableHashSetValue
{
    public static ImmutableHashSetValue<T> WithValueSemantics<T>(this ImmutableHashSet<T> value)
        => new ImmutableHashSetValue<T>(value);

    public static ImmutableHashSetValue<T> Create<T>(ImmutableArray<T> source, IEqualityComparer<T>? equalityComparer)
    {
        var set = source.ToImmutableHashSet(equalityComparer);
        return new ImmutableHashSetValue<T>(set);
    }
}

public readonly struct ImmutableHashSetValue<T> : IReadOnlyCollection<T>, IEquatable<ImmutableHashSetValue<T>>
{
    private readonly ImmutableHashSet<T> _Value;

    public ImmutableHashSetValue(ImmutableHashSet<T> set)
    {
        _Value = set;
    }

    public ImmutableHashSetValue(IEnumerable<T> items)
    {
        _Value = ImmutableHashSet.CreateRange(items);
    }

    public ImmutableHashSet<T> Value => _Value ?? ImmutableHashSet<T>.Empty;

    public ImmutableHashSetValue<T> Add(T item) => _Value.Add(item).WithValueSemantics();

    public ImmutableHashSetValue<T> AddRange(IEnumerable<T> items) => _Value.Union(items).WithValueSemantics();

    public ImmutableHashSetValue<T> Remove(T item) => _Value is not { Count: > 0 } ? this : _Value.Remove(item).WithValueSemantics();

    public ImmutableHashSetValue<T> Except(IEnumerable<T> items) => _Value is not { Count: > 0 } ? this : _Value.Except(items).WithValueSemantics();

    public bool Contains(T item) => _Value is not null && _Value.Contains(item);

    public int Count => _Value is null ? 0 : _Value.Count;

    public ImmutableHashSet<T>.Enumerator GetEnumerator() => Value.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => _Value is null ? Enumerable.Empty<T>().GetEnumerator() : _Value.GetEnumerator();

    public override bool Equals(object? obj) => obj is ImmutableHashSetValue<T> other && Equals(other);

    public bool Equals(ImmutableHashSetValue<T> other) => _Value is null ? other.Count == 0 : _Value.SetEquals(other._Value);

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

    public static implicit operator ImmutableHashSet<T>(ImmutableHashSetValue<T> value) => value._Value;
    public static implicit operator ImmutableHashSetValue<T>(ImmutableHashSet<T> value) => new ImmutableHashSetValue<T>(value);
}
