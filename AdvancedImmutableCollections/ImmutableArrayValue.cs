namespace AdvancedImmutableCollections;

/// <summary>
/// Provides extension methods for <see cref="ImmutableArray{T}"/> to enable value semantics.
/// </summary>
public static class ImmutableArrayValue
{
    /// <summary>
    /// Creates a new <see cref="ImmutableArrayValue{T}"/> instance from the specified <paramref name="array"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <returns></returns>
    public static ImmutableArrayValue<T> WithValueSemantics<T>(in this ImmutableArray<T> array) => new ImmutableArrayValue<T>(array);

    /// <summary>
    /// Evaluates whether two <see cref="ImmutableArray{T}"/> instances are equal using <see cref="EqualityComparer{T}.Default"/> for comparing items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="other"></param>
    /// <returns></returns>
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

    /// <summary>
    /// Evaluates whether two <see cref="ImmutableArray{T}"/> instances are equal using the specified <paramref name="equalityComparer"/> for comparing items.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="other"></param>
    /// <param name="equalityComparer"></param>
    /// <returns></returns>
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

#if NET8_0_OR_GREATER
    public static ImmutableArrayValue<T> Create<T>(ReadOnlySpan<T> items) // this method is used as a collection builder
    => ImmutableArray.Create(items).WithValueSemantics(); 
#endif
}
