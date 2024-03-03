using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using AdvancedImmutableCollections;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedImmutableCollections;

[TestClass]
public class ImmutableArrayValueTests
{
    [TestMethod]
    public void WithValueSemanticsTest()
    {
        var value = ImmutableArray.Create(1, 2, 3);
        var actual = ImmutableArrayValue.WithValueSemantics(value);

        CollectionAssert.AreEqual(value.ToArray(), actual.Value.ToArray());
    }

    /// <summary>
    /// Verifies <see cref="ImmutableArrayValue.SequenceEqual{T}(ImmutableArray{T}, ImmutableArray{T})"/>
    /// </summary>
    /// <param name="array"></param>
    /// <param name="other"></param>
    [TestMethod]
    [DynamicData(nameof(SequenceEqualTestData), DynamicDataSourceType.Method, DynamicDataDisplayName = nameof(DynamicData.GetDynamicDataDisplayName), DynamicDataDisplayNameDeclaringType = typeof(DynamicData))]
    public void SequenceEqualTest(int[] arrayData, int[] otherData, bool expected, TestCaseInfo testCase)
    {
        testCase.Execute(() =>
        {
            var array = arrayData.ToImmutableArray();
            var other = otherData.ToImmutableArray();
            var actual = ImmutableArrayValue.SequenceEqual(array, other);
            Assert.AreEqual(expected, actual);
        });
    }

    private static IEnumerable<object?[]> SequenceEqualTestData()
    {
        return
            [
                DynamicData.TestCase().WithArray(Array.Empty<int>()).WithArray(Array.Empty<int>()).With(true),
                DynamicData.TestCase().WithArray(1).WithArray(Array.Empty<int>()).With(false),
                DynamicData.TestCase().WithArray(Array.Empty<int>()).WithArray(2).With(false),
                DynamicData.TestCase().WithArray(0).WithArray(2).With(false),
                DynamicData.TestCase().WithArray(1, 2, 3).WithArray(1, 2, 3).With(true),
                DynamicData.TestCase().WithArray(1, 3, 2).WithArray(1, 2, 3).With(false),
                DynamicData.TestCase().WithArray(1, 2, 3).WithArray(1, 2, 4).With(false),
                DynamicData.TestCase().WithArray(2, 2, 3).WithArray(1, 2, 3).With(false),
                DynamicData.TestCase().WithArray(1, 2, 3).WithArray(1, 2).With(false),
                DynamicData.TestCase().WithArray(1, 2).WithArray(1, 2, 3).With(false),
                DynamicData.TestCase().WithArray(1, 2, 3).WithArray(1, 2, 3, 4).With(false),
                DynamicData.TestCase().WithArray(1, 2, 3, 4).WithArray(1, 2, 3).With(false),
            ];
    }

    [TestMethod]
    public void SequenceEqualWithComparerTest()
    {
        ImmutableArray<string> a = ImmutableArray.Create("a");
        ImmutableArray<string> ab = ImmutableArray.Create("a", "b");
        ImmutableArray<string> abc = ImmutableArray.Create("a", "b", "c");
        ImmutableArray<string> aBc = ImmutableArray.Create("a", "B", "c");

        SequenceEqualWithComparerTest(abc, abc, StringComparer.Ordinal, true);
        SequenceEqualWithComparerTest(abc, ab, StringComparer.Ordinal, false); // different length        
        SequenceEqualWithComparerTest(a, ab, StringComparer.Ordinal, false); // different length
        SequenceEqualWithComparerTest(abc, aBc, StringComparer.Ordinal, false); // different casing
        SequenceEqualWithComparerTest(abc, aBc, StringComparer.OrdinalIgnoreCase, true);
        SequenceEqualWithComparerTest(ImmutableArray<string>.Empty, ImmutableArray<string>.Empty, StringComparer.Ordinal, true);
        SequenceEqualWithComparerTest(ImmutableArray<string>.Empty, default, StringComparer.Ordinal, true);
        SequenceEqualWithComparerTest(default, ImmutableArray<string>.Empty, StringComparer.Ordinal, true);
        SequenceEqualWithComparerTest(default, default, StringComparer.Ordinal, true);
        SequenceEqualWithComparerTest(a, default, StringComparer.Ordinal, false);
        SequenceEqualWithComparerTest(default, a, StringComparer.Ordinal, false);

        static void SequenceEqualWithComparerTest(ImmutableArray<string> testObject, ImmutableArray<string> other, IEqualityComparer<string> comparer, bool expected)
        {
            var actual = testObject.SequenceEqual(other, comparer);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void Create_Array_Test()
    {
        int[] items = [1, 2, 3];
        var arrayValue = ImmutableArrayValue.Create(items);

        Assert.AreEqual(3, arrayValue.Length);
        Assert.AreEqual(1, arrayValue[0]);
        Assert.AreEqual(2, arrayValue[1]);
        Assert.AreEqual(3, arrayValue[2]);
    }

#if NET8_0_OR_GREATER
    [TestMethod]
    public void CreateTest()
    {
        ReadOnlySpan<int> items = [1, 2, 3];
        var arrayValue = ImmutableArrayValue.Create(items);

        Assert.AreEqual(3, arrayValue.Length);
        Assert.AreEqual(1, arrayValue[0]);
        Assert.AreEqual(2, arrayValue[1]);
        Assert.AreEqual(3, arrayValue[2]);
    }
#endif

    #region IStructuralEquatable
    [TestMethod]
    public void IStructuralEquatable_EqualsTest()
    {
        var ab = ImmutableArray.Create("a", "b");
        var abc = ImmutableArray.Create("a", "b", "c");
        var aBc = ImmutableArray.Create("a", "B", "c");

        IStructuralEquatable_EqualsTest(aBc, (ImmutableArrayValue<string>)aBc, StringComparer.Ordinal, true);
        IStructuralEquatable_EqualsTest(abc, (ImmutableArrayValue<string>)aBc, StringComparer.Ordinal, false);
        IStructuralEquatable_EqualsTest(abc, (ImmutableArrayValue<string>)aBc, StringComparer.OrdinalIgnoreCase, true);
        IStructuralEquatable_EqualsTest(abc, (ImmutableArrayValue<string>)ab, StringComparer.Ordinal, false);

        IStructuralEquatable_EqualsTest(aBc, aBc, StringComparer.Ordinal, true);
        IStructuralEquatable_EqualsTest(aBc, abc, StringComparer.Ordinal, false);
        IStructuralEquatable_EqualsTest(aBc, abc, StringComparer.OrdinalIgnoreCase, true);
        IStructuralEquatable_EqualsTest(ab, abc, StringComparer.Ordinal, false);

        IStructuralEquatable_EqualsTest(aBc, new[] { "a", "B", "c" }, StringComparer.Ordinal, true);
        IStructuralEquatable_EqualsTest(aBc, new[] { "a", "b", "c" }, StringComparer.Ordinal, false);

        static void IStructuralEquatable_EqualsTest(ImmutableArrayValue<string> testObject, object? other, IEqualityComparer equalityComparer, bool expected)
        {
            IStructuralEquatable testObjectIStructuralEquatable = testObject;
            var actual = testObjectIStructuralEquatable.Equals(other, equalityComparer);
            Assert.AreEqual(expected, actual);
        }
    }

    [TestMethod]
    public void IStructuralEquatable_GetHashcodeTest()
    {
        var hashcodes = new HashSet<int>();

        VerifyHashCode("a");
        VerifyHashCode("A");
        VerifyHashCode("a", "b");
        VerifyHashCode("a", "b", "c");
        VerifyHashCode("a", "B", "c");

        VerifyHashCodeCore(default);

        void VerifyHashCode(params string[] items)
        {
            var testObject = ImmutableArrayValue.Create(items);
            VerifyHashCodeCore(testObject);
        }

        void VerifyHashCodeCore(ImmutableArrayValue<string> testObject)
        {
            IStructuralEquatable testObjectAsIStructuralEquatable = testObject;
            var actual = testObjectAsIStructuralEquatable.GetHashCode(StringComparer.Ordinal);
            Assert.IsTrue(hashcodes.Add(actual), "hashcode is not unique");
        }
    }
    #endregion

    #region IStructuralComparable
    [TestMethod]
    public void IStructuralComparable_CompareToTest()
    {
        var a = ImmutableArray.Create("a");
        var A = ImmutableArray.Create("A");
        var ab = ImmutableArray.Create("a", "b");
        var abc = ImmutableArray.Create("a", "b", "c");
        var aBc = ImmutableArray.Create("a", "B", "c");

        VerifyCompareTo(a, (ImmutableArrayValue<string>)a, StringComparer.Ordinal, 0);
        VerifyCompareTo(a, (ImmutableArrayValue<string>)A, StringComparer.Ordinal, 'a' - 'A'); // a - A = 32
        VerifyCompareTo(a, (ImmutableArrayValue<string>)A, StringComparer.OrdinalIgnoreCase, 0);
        VerifyCompareTo(A, (ImmutableArrayValue<string>)a, StringComparer.Ordinal, 'A' - 'a'); // A - a = -32
        VerifyCompareToThrowsArgumentException(ab, (ImmutableArrayValue<string>)abc, StringComparer.Ordinal); // different length

        VerifyCompareTo(abc, abc, StringComparer.Ordinal, 0);
        VerifyCompareTo(abc, aBc, StringComparer.Ordinal, 'b' - 'B');
        VerifyCompareTo(aBc, abc, StringComparer.Ordinal, 'B' - 'b');
        VerifyCompareTo(aBc, abc, StringComparer.OrdinalIgnoreCase, 0);

        VerifyCompareTo(default, default(ImmutableArrayValue<string>), StringComparer.Ordinal, 0);
        VerifyCompareTo(default, default(ImmutableArray<string>), StringComparer.Ordinal, 0);
        VerifyCompareTo(default, ImmutableArray<string>.Empty, StringComparer.Ordinal, 0);
        VerifyCompareTo(default, Array.Empty<string>(), StringComparer.Ordinal, 0);
        VerifyCompareTo(ImmutableArray<string>.Empty, Array.Empty<string>(), StringComparer.Ordinal, 0);
        VerifyCompareToThrowsArgumentException(ImmutableArray<string>.Empty, a, StringComparer.Ordinal); // different length
        VerifyCompareToThrowsArgumentException(default, (ImmutableArrayValue<string>)aBc, StringComparer.Ordinal); // different length

        static void VerifyCompareTo(ImmutableArrayValue<string> testObject, object? other, IComparer comparer, int expected)
        {
            IStructuralComparable testObjectIStructuralComparable = testObject;
            var actual = testObjectIStructuralComparable.CompareTo(other, comparer);
            Assert.AreEqual(expected, actual);
        }

        static void VerifyCompareToThrowsArgumentException(ImmutableArrayValue<string> testObject, object? other, IComparer comparer)
        {
            IStructuralComparable testObjectIStructuralComparable = testObject;
            Assert.ThrowsException<ArgumentException>(() => testObjectIStructuralComparable.CompareTo(other, comparer));
        }
    }
    #endregion
}
