using Microsoft.VisualStudio.TestTools.UnitTesting;
using AdvancedImmutableCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableArrayComparer{T}"/>
/// </summary>
[TestClass()]
public class ImmutableArrayComparerTests
{
    private static readonly ImmutableArray<string?> a = ImmutableArray.Create<string?>("a");
    private static readonly ImmutableArray<string?> A = ImmutableArray.Create<string?>("A");
    private static readonly ImmutableArray<string?> ab = ImmutableArray.Create<string?>("a", "b");
    private static readonly ImmutableArray<string?> ba = ImmutableArray.Create<string?>("b", "a");
    private static readonly ImmutableArray<string?> abc = ImmutableArray.Create<string?>("a", "b", "c");
    private static readonly ImmutableArray<string?> Abc = ImmutableArray.Create<string?>("A", "b", "c");
    private static readonly ImmutableArray<string?> bca = ImmutableArray.Create<string?>("b", "c", "a");
    private static readonly ImmutableArray<string?> null1 = ImmutableArray.Create((string?)null);
    private static readonly ImmutableArray<string?> null2 = ImmutableArray.Create("a", null);
    private static readonly ImmutableArray<string?> null3 = ImmutableArray.Create(null, "b");

    [TestMethod()]
    public void CtorTest()
    {
        CtorTest((EqualityComparer<int>?)null, EqualityComparer<int>.Default);
        CtorTest(EqualityComparer<int>.Default, EqualityComparer<int>.Default);
        CtorTest(StringComparer.OrdinalIgnoreCase, StringComparer.OrdinalIgnoreCase);

        void CtorTest<T>(IEqualityComparer<T>? itemComparer, IEqualityComparer<T> expectedItemComparer)
        {
            if (expectedItemComparer is null)
            {
                throw new ArgumentNullException(nameof(expectedItemComparer));
            }

            var actual = new ImmutableArrayComparer<T>(itemComparer);
            Assert.AreSame(expectedItemComparer, actual.ItemComparer);
        }
    }

    [TestMethod]
    public void DefaultTest()
    {
        var actual1 = ImmutableArrayComparer<string?>.Default;
        Assert.IsNotNull(actual1);
        Assert.AreEqual(EqualityComparer<string?>.Default, actual1.ItemComparer);
        var actual2 = ImmutableArrayComparer<string?>.Default;
        Assert.AreSame(actual1, actual2);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayComparer{T}.Equals(ImmutableArray{T}, ImmutableArray{T})"/>
    /// </summary>
    [TestMethod()]
    public void Equals_x_y_Test()
    {
        EqualsTestWithStrings(StringComparer.OrdinalIgnoreCase, ignoreCase:true);
        EqualsTestWithStrings(StringComparer.Ordinal, ignoreCase: false);

        void EqualsTestWithStrings(IEqualityComparer<string?> itemComparer, bool ignoreCase)
        {
            var testObject = new ImmutableArrayComparer<string?>(itemComparer);
            var emptyArray = ImmutableArray<string?>.Empty;

            EqualsTest(testObject, a, a, true);
            EqualsTest(testObject, a, A, ignoreCase);
            EqualsTest(testObject, a, ab, false);
            EqualsTest(testObject, a, ba, false);
            EqualsTest(testObject, a, abc, false);
            EqualsTest(testObject, a, Abc, false);
            EqualsTest(testObject, a, null1, false);
            EqualsTest(testObject, a, null3, false);
            EqualsTest(testObject, a, emptyArray, false);

            EqualsTest(testObject, A, a, ignoreCase);
            EqualsTest(testObject, A, A, true);

            EqualsTest(testObject, ab, a, false);
            EqualsTest(testObject, ab, ab, true);
            EqualsTest(testObject, ab, ba, false);
            EqualsTest(testObject, ab, abc, false);
            EqualsTest(testObject, ab, null1, false);
            EqualsTest(testObject, ab, null2, false);
            EqualsTest(testObject, ab, null3, false);
            EqualsTest(testObject, ab, emptyArray, false);

            EqualsTest(testObject, ba, a, false);
            EqualsTest(testObject, ba, A, false);
            EqualsTest(testObject, ba, ba, true);
            EqualsTest(testObject, ba, abc, false);
            EqualsTest(testObject, ba, bca, false);
            EqualsTest(testObject, ba, null1, false);
            EqualsTest(testObject, ba, null2, false);

            EqualsTest(testObject, abc, a, false);
            EqualsTest(testObject, abc, ab, false);
            EqualsTest(testObject, abc, abc, true);
            EqualsTest(testObject, abc, Abc, ignoreCase);
            EqualsTest(testObject, abc, bca, false);

            EqualsTest(testObject, null1, a, false);
            EqualsTest(testObject, null1, ab, false);
            EqualsTest(testObject, null1, abc, false);
            EqualsTest(testObject, null1, null1, true);
            EqualsTest(testObject, null1, null2, false);
            EqualsTest(testObject, null1, null3, false);
            EqualsTest(testObject, null1, emptyArray, false);

            EqualsTest(testObject, null2, a, false);
            EqualsTest(testObject, null2, ab, false);
            EqualsTest(testObject, null2, abc, false);
            EqualsTest(testObject, null2, null1, false);
            EqualsTest(testObject, null2, null2, true);
            EqualsTest(testObject, null2, null3, false);
            EqualsTest(testObject, null2, emptyArray, false);

            EqualsTest(testObject, null3, a, false);
            EqualsTest(testObject, null3, ab, false);
            EqualsTest(testObject, null3, abc, false);
            EqualsTest(testObject, null3, null1, false);
            EqualsTest(testObject, null3, null2, false);
            EqualsTest(testObject, null3, null3, true);
            EqualsTest(testObject, null3, emptyArray, false);

            EqualsTest(testObject, emptyArray, emptyArray, true);
            EqualsTest(testObject, emptyArray, a, false);
            EqualsTest(testObject, emptyArray, ab, false);
            EqualsTest(testObject, emptyArray, abc, false);
            EqualsTest(testObject, emptyArray, null1, false);
            EqualsTest(testObject, emptyArray, null3, false);
        }

        void EqualsTest<T>(ImmutableArrayComparer<T?> testObject, ImmutableArray<T?> x, ImmutableArray<T?> y, bool expected)
        {
            var actual = testObject.Equals(x, y);
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayComparer{T}.GetHashCode(ImmutableArray{T})"/>
    /// </summary>
    [TestMethod()]
    public void GetHashCode_Uniqueness_Test()
    {
        var testObject = new ImmutableArrayComparer<string?>(StringComparer.Ordinal);
        ImmutableArray<string?>[] arrays = [a, A, ab, ba, abc, Abc, bca, null1, null2, null3];
        int[] hashCodes = new int[arrays.Length];
        for (int i = 0; i < arrays.Length; i++)
        {
            hashCodes[i] = testObject.GetHashCode(arrays[i]);
        }

        var uniqueHashCodes = new HashSet<int>(hashCodes);
        Assert.AreEqual(arrays.Length, uniqueHashCodes.Count);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayComparer{T}.Equals(object?)"/>
    /// </summary>
    [TestMethod()]
    public void EqualsTest()
    {
        var comparer1 = new ImmutableArrayComparer<string?>(StringComparer.OrdinalIgnoreCase);
        var comparer2 = new ImmutableArrayComparer<string?>(StringComparer.Ordinal);
        var comparer3 = new ImmutableArrayComparer<string?>(null);
        var comparer4 = new ImmutableArrayComparer<int?>(null);
        var allComparers = new object[] { comparer1, comparer2, comparer3, comparer4 };

        EqualsTest(comparer1, 0);
        EqualsTest(comparer2, 1);
        EqualsTest(comparer3, 2);
        EqualsTest(comparer4, 3);

        // test with different instance
        Assert.AreEqual(new ImmutableArrayComparer<int?>(null), comparer4);

        void EqualsTest<T>(ImmutableArrayComparer<T> testObject, int index)
        {
            for (int i = 0; i < allComparers.Length; i++)
            {
                var other = allComparers[i];
                bool expected = index == i;
                bool actual = testObject.Equals(other);
                Assert.AreEqual(expected, actual);
            }
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayComparer{T}.GetHashCode()"/>
    /// </summary>
    [TestMethod()]
    public void GetHashCodeTest()
    {
        var comparer1 = new ImmutableArrayComparer<string?>(StringComparer.OrdinalIgnoreCase);
        var comparer2 = new ImmutableArrayComparer<string?>(StringComparer.Ordinal);
        var comparer3 = new ImmutableArrayComparer<string?>(null);
        var comparer4 = new ImmutableArrayComparer<int?>(null);
        var allComparers = new object[] { comparer1, comparer2, comparer3, comparer4 };

        var hashCodes = new int[allComparers.Length];
        for (int i = 0; i < allComparers.Length; i++)
        {
            hashCodes[i] = allComparers[i].GetHashCode();
        }

        var uniqueHashCodes = new HashSet<int>(hashCodes);
        Assert.AreEqual(allComparers.Length, uniqueHashCodes.Count);
    }
}