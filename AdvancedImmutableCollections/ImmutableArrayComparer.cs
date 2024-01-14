namespace AdvancedImmutableCollections;

/// <summary>
/// Implements <see cref="IEqualityComparer{T}"/> for <see cref="ImmutableArray{T}"/>.
/// Two arrays are considered equal if they have the same length and all their elements are equal accoring to the <see cref="ItemComparer"/>.
/// </summary>
/// <typeparam name="T"></typeparam>
public sealed class ImmutableArrayComparer<T> : IEqualityComparer<ImmutableArray<T>>
{
    /// <summary>
    /// Initializes a new instance with the specified <paramref name="itemComparer"/>.
    /// </summary>
    /// <param name="itemComparer">The comparer that is used to compare the items of arrays. If <see langword="null"/>, <see cref="EqualityComparer{T}.Default"/> will be used.</param>
    public ImmutableArrayComparer(IEqualityComparer<T>? itemComparer)
    {
        ItemComparer = itemComparer ?? EqualityComparer<T>.Default;
    }

    /// <summary>
    /// Initializes a new instance with the <see cref="EqualityComparer{T}.Default">default comparer</see> for <typeparamref name="T"/> as <see cref="ItemComparer"/>.
    /// </summary>
    private ImmutableArrayComparer() { ItemComparer = EqualityComparer<T>.Default; }

    /// <summary>
    /// The comparer that is used to compare the items of arrays.
    /// </summary>
    public IEqualityComparer<T> ItemComparer { get; }

    /// <summary>
    /// The default instance of <see cref="ImmutableArrayComparer{T}"/>, which uses the <see cref="EqualityComparer{T}.Default">default comparer</see> for <typeparamref name="T"/> as <see cref="ItemComparer"/>.
    /// </summary>
    public static readonly ImmutableArrayComparer<T> Default = new ImmutableArrayComparer<T>();

    public bool Equals(ImmutableArray<T> x, ImmutableArray<T> y)
    {
        switch (x.IsDefaultOrEmpty, y.IsDefaultOrEmpty)
        {
            case (true, true):
                return true;
            case (true, false):
            case (false, true):
                return false;
        }
        if (x.Length != y.Length)
        {
            return false;
        }

        for (var i = 0; i < x.Length; i++)
        {
            if (!ItemComparer.Equals(x[i], y[i]))
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(ImmutableArray<T> obj) => obj.GetSequenceHashCode();

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(this, obj))
        {
            return true;
        }
        return obj is ImmutableArrayComparer<T> other && ItemComparer.Equals(other.ItemComparer);
    }

    public override int GetHashCode() => ItemComparer.GetHashCode();
}
