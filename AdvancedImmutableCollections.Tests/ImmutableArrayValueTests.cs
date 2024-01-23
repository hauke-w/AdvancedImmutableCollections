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
        var array1 = ImmutableArray.Create("a", "b", "c");
        var array2 = ImmutableArray.Create("A", "B", "C");
        var array3 = ImmutableArray.Create("A", "B", "D");

        Assert.IsTrue(array1.SequenceEqual(array2, StringComparer.OrdinalIgnoreCase));
        Assert.IsFalse(array1.SequenceEqual(array3, StringComparer.OrdinalIgnoreCase));
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
