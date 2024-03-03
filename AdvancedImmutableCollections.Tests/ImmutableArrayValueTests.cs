using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using AdvancedImmutableCollections;
using AdvancedImmutableCollections.Tests.Util;

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
}
