using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests.CollectionAdapters;

/// <summary>
/// Collection adapter for <see cref="IImmutableSet{T}"/>
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IImmutableSetAdapter<T> : IImmutableCollectionAdapter<T>
{
    IImmutableSet<T> Except(IEnumerable<T> other);
    IImmutableSet<T> Intersect(IEnumerable<T> other);
    bool IsProperSubsetOf(IEnumerable<T> other);
    bool IsProperSupersetOf(IEnumerable<T> other);
    bool IsSubsetOf(IEnumerable<T> other);
    bool IsSupersetOf(IEnumerable<T> other);
    bool Overlaps(IEnumerable<T> other);
    bool SetEquals(IEnumerable<T> other);
    IImmutableSet<T> SymmetricExcept(IEnumerable<T> other);
    bool TryGetValue(T equalValue, out T actualValue);
    IImmutableSet<T> Union(IEnumerable<T> other);
}
