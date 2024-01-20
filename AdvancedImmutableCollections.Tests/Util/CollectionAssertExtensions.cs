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
        var expectedItems = new Dictionary<T, int>(itemComparer);
        int expectedNullCount = 0;
        int expectedCount = 0;
        foreach (var item in expected)
        {
            expectedCount++;
            if (item is null)
            {
                expectedNullCount++;
            }
            else if (expectedItems.TryGetValue(item, out int count))
            {
                expectedItems[item] = count + 1;
            }
            else
            {
                expectedItems.Add(item, 1);
            }
        }

        var remainingItems = new Dictionary<T, int>(expectedItems, itemComparer);
        int remainingNullCount = 0;
        int actualCount = 0;
        foreach (var item in actual)
        {
            actualCount++;
            if (item is null)
            {
                if (remainingNullCount <= 0)
                {
                    Assert.Fail($"null value occurs more than expect: {expectedNullCount}");
                }
                remainingNullCount--;
            }
            else if (remainingItems.TryGetValue(item, out int count))
            {
                if (count <= 0)
                {
                    Assert.Fail($"item {item} occurs more often than expected: {expectedItems[item]}");
                }
                remainingItems[item] = count - 1;
            }
            else
            {
                Assert.Fail($"item {item} does not occur in expected items");
            }
        }
        Assert.AreEqual(0, remainingNullCount, "null value does not occur as often as expected");
        Assert.AreEqual(expectedCount, actualCount);
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
