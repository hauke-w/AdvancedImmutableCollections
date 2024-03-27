using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ImmutableSortedSetValueAdapter<T>(ImmutableSortedSetValue<T> collection) : ImmutableCollectionAdapter<T, ImmutableSortedSetValue<T>>(collection), IImmutableSetAdapter<T>
{
    public ImmutableSortedSetValueAdapter(params T[] items) : this(ImmutableSortedSet.Create(items)) { }
    public ImmutableSortedSetValueAdapter(IComparer<T>? comparer, params T[] items) : this(ImmutableSortedSet.Create(comparer, items)) { }

    public int Count => Collection.Count;

    public IReadOnlyCollection<T> Add(T value) => Collection.Add(value);
    public IReadOnlyCollection<T> AddRange(IEnumerable<T> items) => Collection.Union(items);
    public IReadOnlyCollection<T> Clear() => Collection.Clear();
    public bool Contains(T item) => Collection.Contains(item);
    public IImmutableSet<T> Except(IEnumerable<T> other) => Collection.Except(other);
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        // wrap strong enumerator ImmutableSortedSet<T>.Enumerator into IEnumerator<T>
        var inner = Collection.GetEnumerator();
        Assert.IsNotNull(inner);
        while (inner.MoveNext())
        {
            yield return inner.Current;
        }
    }
    public IImmutableSet<T> Intersect(IEnumerable<T> other) => Collection.Intersect(other);
    public bool IsProperSubsetOf(IEnumerable<T> other) => Collection.IsProperSubsetOf(other);
    public bool IsProperSupersetOf(IEnumerable<T> other) => Collection.IsProperSupersetOf(other);
    public bool IsSubsetOf(IEnumerable<T> other) => Collection.IsSubsetOf(other);
    public bool IsSupersetOf(IEnumerable<T> other) => Collection.IsSupersetOf(other);
    public bool Overlaps(IEnumerable<T> other) => Collection.Overlaps(other);
    public IReadOnlyCollection<T> Remove(T value) => Collection.Remove(value);
    public bool SetEquals(IEnumerable<T> other) => Collection.SetEquals(other);
    public IImmutableSet<T> SymmetricExcept(IEnumerable<T> other) => Collection.SymmetricExcept(other);
    public bool TryGetValue(T equalValue, out T actualValue) => Collection.TryGetValue(equalValue, out actualValue);
    public IImmutableSet<T> Union(IEnumerable<T> other) => Collection.Union(other);
    IEnumerator IEnumerable.GetEnumerator() => Collection.GetEnumerator();
}
