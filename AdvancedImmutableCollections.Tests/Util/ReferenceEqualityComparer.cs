#if NETFRAMEWORK || !NET6_0_OR_GREATER
using System.Runtime.CompilerServices;

namespace System.Collections.Generic;

/// <summary>
/// Eqauality comparer that uses <see cref="object.ReferenceEquals(object?, object?)"/> to compare items
/// </summary>
internal sealed class ReferenceEqualityComparer : IEqualityComparer<object?>
{
    private ReferenceEqualityComparer() { }

    public static readonly ReferenceEqualityComparer Instance = new();

    public new bool Equals(object? x, object? y) => ReferenceEquals(x, y);
    public int GetHashCode(object? obj) => RuntimeHelpers.GetHashCode(obj!);
}

#endif