using System.Collections;
using System.Runtime.CompilerServices;

namespace AdvancedImmutableCollections;

/// <summary>
/// Immutable array with value semantics i.e. the <see cref="Equals(ImmutableArrayValue{T})"/> method returns <see langword="true"/> if all items are equal.
/// </summary>
/// <typeparam name="T"></typeparam>
[Serializable]
#if NET8_0_OR_GREATER
[CollectionBuilder(typeof(ImmutableArrayValue), nameof(ImmutableArrayValue.Create))] 
#endif
public readonly struct ImmutableArrayValue<T> : IImmutableList<T>, IEquatable<ImmutableArrayValue<T>>, IStructuralComparable
{
    public ImmutableArrayValue(ImmutableArray<T> value)
    {
        _Value = value;
    }

    public T this[int index] => _Value.IsDefault ? throw new IndexOutOfRangeException() : _Value[index];

    private readonly ImmutableArray<T> _Value;

    /// <summary>
    /// Returns <see cref="_Value"/> if it is not default, otherwise returns <see cref="ImmutableArray{T}.Empty"/>."/>
    /// </summary>
    public ImmutableArray<T> Value => _Value.IsDefault ? ImmutableArray<T>.Empty : _Value;

    public bool IsDefault => _Value.IsDefault;
    public bool IsDefaultOrEmpty => _Value.IsDefaultOrEmpty;

    private ImmutableArray<T> InitializedValueOrArgumentOutOfRange => _Value.IsDefault ? throw new ArgumentOutOfRangeException() : _Value;

    public int Length => _Value.IsDefault ? 0 : _Value.Length;

    int IReadOnlyCollection<T>.Count => Length;

    public ImmutableArrayValue<T> Add(T value) => Value.Add(value).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.Add(T value) => Add(value);

    public ImmutableArrayValue<T> AddRange(IEnumerable<T> items) => Value.AddRange(items).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.AddRange(IEnumerable<T> items) => AddRange(items);

    public bool Contains(T value) => !_Value.IsDefault && _Value.Contains(value);

    public ImmutableArrayValue<T> Clear() => _Value.IsDefaultOrEmpty ? this : _Value.Clear().WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.Clear() => Clear();

    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    {
        if (_Value.IsDefaultOrEmpty)
        {
            return index == 0 && count == 0
                ? -1
                : throw new ArgumentOutOfRangeException();
        }
        return _Value.IndexOf(item, index, count, equalityComparer);
    }

    public IImmutableList<T> Insert(int index, T element) => Value.Insert(index, element).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.Insert(int index, T element) => Insert(index, element);

    public ImmutableArrayValue<T> InsertRange(int index, IEnumerable<T> items) => Value.InsertRange(index, items).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.InsertRange(int index, IEnumerable<T> items) => InsertRange(index, items);

    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer)
    {
        if (_Value.IsDefaultOrEmpty)
        {
            return index == 0 && count == 0
                ? -1
                : throw new ArgumentOutOfRangeException();
        }
        return _Value.LastIndexOf(item, index, count, equalityComparer);
    }

    public ImmutableArrayValue<T> Remove(T value, IEqualityComparer<T>? equalityComparer) => _Value.IsDefaultOrEmpty ? this : _Value.Remove(value, equalityComparer).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.Remove(T value, IEqualityComparer<T>? equalityComparer) => Remove(value, equalityComparer);

    public ImmutableArrayValue<T> RemoveAll(Predicate<T> match) => _Value.IsDefaultOrEmpty ? this : _Value.RemoveAll(match).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.RemoveAll(Predicate<T> match) => RemoveAll(match);

    public ImmutableArrayValue<T> RemoveAt(int index) => InitializedValueOrArgumentOutOfRange.RemoveAt(index).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.RemoveAt(int index) => RemoveAt(index);

    public ImmutableArrayValue<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer)
        => _Value.IsDefaultOrEmpty ? this : _Value.RemoveRange(items, equalityComparer).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => RemoveRange(items, equalityComparer);

    public ImmutableArrayValue<T> RemoveRange(int index, int count) => InitializedValueOrArgumentOutOfRange.RemoveRange(index, count).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.RemoveRange(int index, int count) => RemoveRange(index, count);

    public ImmutableArrayValue<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => _Value.IsDefaultOrEmpty ? this : _Value.Replace(oldValue, newValue, equalityComparer).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Replace(oldValue, newValue, equalityComparer);

    public ImmutableArrayValue<T> SetItem(int index, T value) => InitializedValueOrArgumentOutOfRange.SetItem(index, value).WithValueSemantics();
    IImmutableList<T> IImmutableList<T>.SetItem(int index, T value) => SetItem(index, value);

    public override bool Equals([NotNullWhen(true)] object? obj)
        => obj is ImmutableArrayValue<T> other && Equals(other);
    public bool Equals(ImmutableArrayValue<T> other)
        => _Value.SequenceEqual(other._Value);

    public override int GetHashCode() => _Value.IsDefaultOrEmpty ? 0 : _Value.GetSequenceHashCode();

    public ImmutableArray<T>.Enumerator GetEnumerator() => Value.GetEnumerator();

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => _Value.IsDefault ? Enumerable.Empty<T>().GetEnumerator() : ((IEnumerable<T>)_Value).GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => _Value.IsDefault ? Enumerable.Empty<T>().GetEnumerator() : ((IEnumerable)_Value).GetEnumerator();

    public static implicit operator ImmutableArray<T>(ImmutableArrayValue<T> value) => value._Value;
    public static implicit operator ImmutableArrayValue<T>(ImmutableArray<T> value) => new ImmutableArrayValue<T>(value);

    int IStructuralComparable.CompareTo(object? other, IComparer comparer)
    {
        if (_Value.IsDefault)
        {
            return other is null or IReadOnlyCollection<T> { Count: 0 }
                ? 0
                : -1;
        }
        return ((IStructuralComparable)_Value).CompareTo(other, comparer);
    }
}
