using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Immutable;
using System.Globalization;

namespace AdvancedImmutableCollections;

[TestClass]
public class ImmutableSortedSetValueTests
{
    [TestMethod]
    [DataRow(new int[] { 1, 2, 3 })]
    [DataRow(new int[] { })]
    public void WithValueSemanticsTest(int[] values)
    {
        ImmutableSortedSet<int> hashSet = values.ToImmutableSortedSet();
        ImmutableSortedSetValue<int> actual = ImmutableSortedSetValue.WithValueSemantics(hashSet);
        Assert.AreSame(hashSet, actual.Value);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue.Create{T}(ImmutableArray{T}, IComparer{T}?)"/>
    /// </summary>
    [TestMethod]
    public void Create_ImmutableArray_IComparer_Test()
    {
        CreateTest([], [], StringComparer.OrdinalIgnoreCase);
        CreateTest(["a"], ["a"], StringComparer.OrdinalIgnoreCase);
        CreateTest(["a", "A"], ["a", "A"], StringComparer.InvariantCulture);
        CreateTest(["a", "A"], ["a"], StringComparer.InvariantCultureIgnoreCase);
        CreateTest(["a", "B", "b", "C", "A", "b"], ["a", "B", "C"], StringComparer.InvariantCultureIgnoreCase);
        CreateTest(["a", "B", "b", "C", "d", "A", "b"], ["a", "A", "b", "B", "C", "d"], StringComparer.InvariantCulture);

        static void CreateTest(string[] sourceItems, string[] expectedItems, IComparer<string>? comparer)
        {
            var source = sourceItems.ToImmutableArray();
            var actual = ImmutableSortedSetValue.Create(source, comparer);
            VerifyCreateResult(actual, expectedItems, comparer);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue.Create{T}(IComparer{T}?, T[])"/>  
    /// </summary>
    [TestMethod]
    public void Create_IComparer_Array_Test()
    {
        CreateTest([], [], StringComparer.OrdinalIgnoreCase);
        CreateTest(["a"], ["a"], StringComparer.OrdinalIgnoreCase);
        CreateTest(["a", "A"], ["a", "A"], StringComparer.InvariantCulture);
        CreateTest(["a", "A"], ["a"], StringComparer.InvariantCultureIgnoreCase);
        CreateTest(["a", "B", "b", "A", "b"], ["a", "B"], StringComparer.InvariantCultureIgnoreCase);
        CreateTest(["a", "B", "b", "A", "b"], ["a", "A", "b", "B"], StringComparer.InvariantCulture);

        static void CreateTest(string[] sourceItems, string[] expectedItems, IComparer<string>? comparer)
        {
            var actual = ImmutableSortedSetValue.Create(comparer, sourceItems);
            VerifyCreateResult(actual, expectedItems, comparer);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue.Create{T}(T[])"/>
    /// </summary>
    [TestMethod]
    public void Create_Array_Test()
    {
        CreateTest([], []);
        CreateTest(["a"], ["a"]);
        CreateTest(["a", "A"], ["A", "a"]);
        CreateTest(["a", "B", "b", "A", "b"], ["A", "B", "a", "b"]);

        static void CreateTest(string[] sourceItems, string[] expectedItems)
        {
            var actual = ImmutableSortedSetValue.Create(sourceItems);
            VerifyCreateResult(actual, expectedItems, Comparer<string>.Default);
        }
    }

#if NET8_0_OR_GREATER
    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue.Create{T}(ReadOnlySpan{T})"/>
    /// </summary>
    [TestMethod]
    public void Create_ReadOnlySpan_Test()
    {
        CreateTest([], []);
        CreateTest(["a"], ["a"]);
        CreateTest(["A", "a"], ["A", "a"]);
        CreateTest(["a", "B", "c", "A", "d", "b", "C"], ["A", "B", "C", "a", "b", "c", "d"]);

        static void CreateTest(string[] sourceItems, string[] expectedItems)
        {
            ReadOnlySpan<string> source = sourceItems;
            var actual = ImmutableSortedSetValue.Create(source);
            VerifyCreateResult(actual, expectedItems, Comparer<string>.Default);
        }
    }
#endif

    private static void VerifyCreateResult(
        ImmutableSortedSetValue<string> actual,
        string[] expectedItems,
        IComparer<string>? equalityComparer)
    {
        Assert.IsNotNull(actual);
        Assert.IsNotNull(actual.Value);
        var expectedComparer = equalityComparer ?? Comparer<string>.Default;
        Assert.AreSame(expectedComparer, actual.Value.KeyComparer);

        Assert.AreEqual(expectedItems.Length, actual.Count);
        foreach (var item in expectedItems)
        {
            Assert.IsTrue(actual.TryGetValue(item, out var actualItem));
            Assert.AreEqual(item, actualItem);
        }
    }

    [TestMethod]
    public void EmptyTest()
    {
        var actual = ImmutableSortedSetValue.Empty<GenericParameterHelper>();
        Assert.IsFalse(actual.IsDefault);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(0, actual.Value.Count);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableSortedSetValue.SetEquals{T}(ImmutableSortedSet{T}, ImmutableSortedSet{T})"/>
    /// </summary>
    [TestMethod]
    public void SetEqualsTest()
    {
        SetEqualsTest(ImmutableSortedSet<string>.Empty, ImmutableSortedSet<string>.Empty, true);
        SetEqualsTest(ImmutableSortedSet.Create("a"), ImmutableSortedSet<string>.Empty, false);
        SetEqualsTest(ImmutableSortedSet<string>.Empty, ImmutableSortedSet.Create("b"), false);
        SetEqualsTest(ImmutableSortedSet.Create("a", "A", "b"), ImmutableSortedSet.Create("a", "b"), false);
        SetEqualsTest(ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a", "A", "b"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "a", "A", "b"), true);
        SetEqualsTest(ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "a", "b"), true);
        SetEqualsTest(ImmutableSortedSet.Create(StringComparer.InvariantCulture, "a", "B"), ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b"), false);

        void SetEqualsTest(ImmutableSortedSet<string> set, ImmutableSortedSet<string> other, bool expected)
        {
            var actual = ImmutableSortedSetValue.SetEquals(set, other);
            Assert.AreEqual(expected, actual);
            // using ImmutableSortedSet<T>.SetEquals method should have equal result
            Assert.AreEqual(set.SetEquals(other), actual, "inconsistent result");
        }
    }
}