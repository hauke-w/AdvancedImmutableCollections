using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

/// <summary>
/// Collection adapter for <see cref="IImmutableList{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IImmutableListAdapter<T> : IImmutableCollectionAdapter<T>
{
    T this[int index] { get; }

    IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer);
    IImmutableList<T> RemoveAll(Predicate<T> match);
    IImmutableList<T> RemoveAt(int index);
    IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer);
    IImmutableList<T> RemoveRange(int index, int count);
    IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer);
    IImmutableList<T> SetItem(int index, T value);
    IImmutableList<T> Insert(int index, T element);
    IImmutableList<T> InsertRange(int index, IEnumerable<T> items);

    int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);
    int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer);
}
