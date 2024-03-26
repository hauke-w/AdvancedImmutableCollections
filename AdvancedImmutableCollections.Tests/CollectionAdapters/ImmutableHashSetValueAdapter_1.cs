using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public sealed class ImmutableHashSetValueAdapter<T>(ImmutableHashSetValue<T> collection) : ImmutableCollectionAdapter<T, ImmutableHashSetValue<T>>(collection), IImmutableSetAdapter<T>
{
    public ImmutableHashSetValueAdapter(params T[] items) : this(ImmutableHashSet.Create(items)) { }
    public ImmutableHashSetValueAdapter(IEqualityComparer<T>? equalityComparer, params T[] items) : this(ImmutableHashSet.Create(equalityComparer, items)) { }

    public int Count => Collection.Count;

    public IReadOnlyCollection<T> Add(T value) => Collection.Add(value);
    public IReadOnlyCollection<T> AddRange(IEnumerable<T> items) => Collection.Union(items);
    public IReadOnlyCollection<T> Clear() => Collection.Clear();
    public IImmutableSet<T> Except(IEnumerable<T> other) => Collection.Except(other);
    IEnumerator<T> IEnumerable<T>.GetEnumerator()
    {
        // wrap strong enumerator ImmutableHashSet<T>.Enumerator into IEnumerator<T>
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
