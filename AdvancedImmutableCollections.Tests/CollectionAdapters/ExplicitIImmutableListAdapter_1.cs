﻿using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

public class ExplicitIImmutableListAdapter<T>(IImmutableList<T> collection) : ExplicitCollectionAdapter<T, IImmutableList<T>>(collection), IImmutableListAdapter<T>
{
    public T this[int index] => Collection[index];
    
    public IReadOnlyCollection<T> Add(T value) => Collection.Add(value);
    public IReadOnlyCollection<T> AddRange(IEnumerable<T> items) => Collection.AddRange(items);
    public IReadOnlyCollection<T> Clear() => Collection.Clear();   
    public bool Contains(T item) => Collection.Contains(item);
    public int IndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => Collection.IndexOf(item, index, count, equalityComparer);
    public IImmutableList<T> Insert(int index, T element) => Collection.Insert(index, element);
    public IImmutableList<T> InsertRange(int index, IEnumerable<T> items) => Collection.InsertRange(index, items);
    public int LastIndexOf(T item, int index, int count, IEqualityComparer<T>? equalityComparer) => Collection.LastIndexOf(item, index, count, equalityComparer);
    public IReadOnlyCollection<T> Remove(T value) => Collection.Remove(value);
    public IImmutableList<T> Remove(T value, IEqualityComparer<T>? equalityComparer) => Collection.Remove(value, equalityComparer);
    public IImmutableList<T> RemoveAll(Predicate<T> match) => Collection.RemoveAll(match);
    public IImmutableList<T> RemoveAt(int index) => Collection.RemoveAt(index);
    public IImmutableList<T> RemoveRange(IEnumerable<T> items, IEqualityComparer<T>? equalityComparer) => Collection.RemoveRange(items, equalityComparer);
    public IImmutableList<T> RemoveRange(int index, int count) => Collection.RemoveRange(index, count);
    public IImmutableList<T> Replace(T oldValue, T newValue, IEqualityComparer<T>? equalityComparer) => Collection.Replace(oldValue, newValue, equalityComparer);
    public IImmutableList<T> SetItem(int index, T value) => Collection.SetItem(index, value);
}