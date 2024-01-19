namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableArray{T}"/> to enable value semantics.
/// </summary>
public static class ImmutableArrayValue
{
    public static ImmutableArrayValue<T> WithValueSemantics<T>(in this ImmutableArray<T> value) => new ImmutableArrayValue<T>(value);

    public static bool SequenceEqual<T>(this ImmutableArray<T> array, ImmutableArray<T> other)
    {
        if (array.IsDefaultOrEmpty)
        {
            return other.IsDefaultOrEmpty;
        }
        if (other.IsDefault
            || array.Length != other.Length)
        {
            return false;
        }
        return SequenceEqualCore(array, other, EqualityComparer<T>.Default);
    }

    public static bool SequenceEqual<T>(in this ImmutableArray<T> array, in ImmutableArray<T> other, in IEqualityComparer<T> equalityComparer)
    {
        if (array.IsDefaultOrEmpty)
        {
            return other.IsDefaultOrEmpty;
        }
        if (other.IsDefault
            || array.Length != other.Length)
        {
            return false;
        }
        return SequenceEqualCore(array, other, equalityComparer);
    }

    private static bool SequenceEqualCore<T>(in ImmutableArray<T> array, in ImmutableArray<T> other, in IEqualityComparer<T> equalityComparer)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (!equalityComparer.Equals(array[i], other[i]))
            {
                return false;
            }
        }

        return true;
    }

    public static ImmutableArrayValue<T> Create<T>(params T[] items)
        => ImmutableArray.Create(items).WithValueSemantics();
}
