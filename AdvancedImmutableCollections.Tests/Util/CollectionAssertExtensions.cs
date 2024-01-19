using System.Collections;

namespace AdvancedImmutableCollections.Tests.Util;

internal static class CollectionAssertExtensions
{
    public static void AreEqual<T>(this CollectionAssert that, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer)
    {
        if (expected is not ICollection expectedItemsAsCollection)
        {
            expectedItemsAsCollection = expected.ToList();
        }
        if (actual is not ICollection actualAsCollection)
        {
            actualAsCollection = actual.ToList();
        }
        IComparer comparer = ItemComparer<T>.Get(itemComparer);
        CollectionAssert.AreEqual(expectedItemsAsCollection, actualAsCollection, comparer);
    }

    public static void AreEquivalent<T>(this CollectionAssert that, IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer)
    {
        // TODO: CollectionAssert does not support AreEquivalent with comparer
        throw new NotImplementedException();
    }

    private class ItemComparer<T> : IComparer
    {
        private ItemComparer(IEqualityComparer<T> equalityComparer)
        {
            EqualityComparer = equalityComparer;
        }

        public static ItemComparer<T> Get(IEqualityComparer<T>? equalityComparer)
        {
            return equalityComparer is null
                ? Default
                : new ItemComparer<T>(equalityComparer);
        }

        public static ItemComparer<T> Default = new ItemComparer<T>(EqualityComparer<T>.Default);

        private readonly IEqualityComparer<T> EqualityComparer;

        public int Compare(object? x, object? y)
        {
            return (x, y) switch
            {
                (null, null) => 0,
                (T tx, T ty) => EqualityComparer.Equals(tx, ty) ? 0 : -1,
                _ => -1,
            };
        }
    }
}
