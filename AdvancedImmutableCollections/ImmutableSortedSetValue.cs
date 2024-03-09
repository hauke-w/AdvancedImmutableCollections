using System.Diagnostics;

namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableSortedSet{T}"/> to enable value semantics. It includes also factory methods for <see cref="ImmutableSortedSetValue{T}"/>.
/// </summary>
public static class ImmutableSortedSetValue
{
    /// <summary>
    /// Creates a new <see cref="ImmutableSortedSetValue{T}"/> from the specified <paramref name="set"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="set"></param>
    /// <returns></returns>
    public static ImmutableSortedSetValue<T> WithValueSemantics<T>(this ImmutableSortedSet<T> set)
        => new ImmutableSortedSetValue<T>(set);

#if NET8_0_OR_GREATER
    /// <summary>
    /// Creates a new <see cref="ImmutableSortedSetValue{T}"/> from the specified <paramref name="items"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="items"></param>
    /// <returns></returns>
    public static ImmutableSortedSetValue<T> Create<T>(ReadOnlySpan<T> items) // this method is used as a collection builder
        => ImmutableSortedSet.Create(items).WithValueSemantics();
#endif

    /// <summary>
    /// Creates a new <see cref="ImmutableSortedSetValue{T}"/> from the specified <paramref name="source"/> items and using the specified <paramref name="comparer"/>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="source"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static ImmutableSortedSetValue<T> Create<T>(ImmutableArray<T> source, IComparer<T>? comparer)
    {
        var set = source.ToImmutableSortedSet(comparer);
        return new ImmutableSortedSetValue<T>(set);
    }

    public static bool SetEquals<T>(ImmutableSortedSet<T> set, ImmutableSortedSet<T> other)
    {
        if (ReferenceEquals(set, other))
        {
            return true;
        }
        if (set.Count == 0)
        {
            return other.Count == 0;
        }
        else if (other.Count == 0)
        {
            return false;
        }

        other = other.WithComparer(set.KeyComparer);
        if (set.Count != other.Count)
        {
            return false;
        }

        return SequenceEqualCore(set, other);
    }

    internal static bool SequenceEqualCore<T>(ImmutableSortedSet<T> set, ImmutableSortedSet<T> other)
    {
        Debug.Assert(set.Count == other.Count);
        var otherEnumerator = other.GetEnumerator();
        var comparer = set.KeyComparer;
        foreach (var item in set)
        {
            otherEnumerator.MoveNext();
            if (comparer.Compare(item, otherEnumerator.Current) != 0)
            {
                return false;
            }
        }
        return true;
    }
}
