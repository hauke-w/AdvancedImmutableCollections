using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdvancedImmutableCollections;
[TestClass]
public class ImmutableSortedSetValue1Tests : ImmutableSetTestsBase<ImmutableSortedSetValue<GenericParameterHelper>, SortedSet<GenericParameterHelper>>
{
    protected override IEqualityTestStrategy EqualityTestStrategy => new SortedSetValueEqualityTestStrategy();

    private class SortedSetValueEqualityTestStrategy : ValueEqualityTestStrategy
    {
        public SortedSetValueEqualityTestStrategy()
        {
            DefaultValueIsEqualEmpty = true;
        }
    }

    protected override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableSortedSetValue<GenericParameterHelper> collection, GenericParameterHelper item)
        => collection.Add(item);
    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(ImmutableSortedSetValue<GenericParameterHelper> collection, params GenericParameterHelper[] newItems)
        => collection.Union(newItems);
    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableSortedSetValue<GenericParameterHelper> collection) => collection.Clear();
    protected override bool Contains(ImmutableSortedSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Contains(item);
    protected override IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems) => new ImmutableSortedSetValue<T>(initialItems);
    protected override IReadOnlyCollection<T>? GetDefaultValue<T>() => default(ImmutableSortedSetValue<T>);
    protected override IEnumerator<GenericParameterHelper> GetEnumerator(ImmutableSortedSetValue<GenericParameterHelper> collection) => collection.GetEnumerator();
    protected override SortedSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new SortedSet<GenericParameterHelper>(initialItems);
    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableSortedSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);
    protected internal override ImmutableSortedSetValue<GenericParameterHelper> CreateInstance() => new ImmutableSortedSetValue<GenericParameterHelper>();
    protected override IImmutableSet<GenericParameterHelper> Except(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Except(other);
    protected override IImmutableSet<GenericParameterHelper> Union(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Union(other);
    protected override IImmutableSet<GenericParameterHelper> Intersect(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Intersect(other);
    protected override IImmutableSet<GenericParameterHelper> SymmetricExcept(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.SymmetricExcept(other);
    protected override bool SetEquals(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.SetEquals(other);
    protected override bool IsProperSubsetOf(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.IsProperSubsetOf(other);

    protected override bool IsProperSupersetOf(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.IsProperSupersetOf(other);

    protected override bool IsSubsetOf(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.IsSubsetOf(other);

    protected override bool IsSupersetOf(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.IsSupersetOf(other);

    protected override bool Overlaps(ImmutableSortedSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other) => collection.Overlaps(other);

    protected override bool TryGetValue(ImmutableSortedSetValue<GenericParameterHelper> collection, GenericParameterHelper equalValue, out GenericParameterHelper actualValue)
        => collection.TryGetValue(equalValue, out actualValue);

    [TestMethod]
    public void IsDefaultTest()
    {
        Assert.IsTrue(default(ImmutableSortedSetValue<GenericParameterHelper>).IsDefault);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>().IsDefault);
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefault);
        GenericParameterHelper[] items = [new()];
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(items).IsDefault);
    }

    [TestMethod]
    public void IsDefaultOrEmptyTest()
    {
        Assert.IsTrue(default(ImmutableSortedSetValue<GenericParameterHelper>).IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>().IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableSortedSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefaultOrEmpty);
        var items = new GenericParameterHelper[] { new GenericParameterHelper() };
        Assert.IsFalse(new ImmutableSortedSetValue<GenericParameterHelper>(items).IsDefaultOrEmpty);
    }

    [TestMethod]
    public override void GetEnumeratorTest()
    {
        foreach (var (testObject, expectedItems) in GetGetEnumeratorTestCases())
        {
            var actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);

            foreach (var item in expectedItems)
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(item, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    [TestMethod]
    public void IEnumerable_T_GetEnumeratorTest()
    {
        foreach ((IEnumerable<string> testObject, string[] expectedItems) in GetGetEnumeratorTestCases())
        {
            var actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);

            foreach (var item in expectedItems)
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(item, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    [TestMethod]
    public override void IEnumerable_GetEnumeratorTest()
    {
        foreach ((IEnumerable testObject, string[] expectedItems) in GetGetEnumeratorTestCases())
        {
            var actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);

            foreach (var item in expectedItems)
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(item, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    private static (ImmutableSortedSetValue<string> TestObject, string[] ExpectedItems)[] GetGetEnumeratorTestCases()
    {
        return
            [
                (default, []),
                (new (), []),
                (new (new []{"b", "a", "z", "f" }), ["a", "b", "f", "z"]),
                (new (StringComparer.InvariantCulture, ["c", "b", "A", "a", "C", "B"]), [ "a", "A", "b", "B", "c", "C"]),
                (new (new []{"123" }), ["123"]),
            ];
    }

    [TestMethod]
    public override void ExceptTest()
    {
        ExceptTest(default, Array.Empty<string>(), default);
        ExceptTest(default, default(ImmutableSortedSetValue<string>), default);
        ExceptTest(default, ["a"], default);
        ExceptTest(default, ImmutableSortedSetValue.Create("a", "b"), default);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableArray<string>), []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a"], []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a", "b"], []);
        ExceptTest(ImmutableSortedSetValue.Create("a"), ["a"], []);
        ExceptTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a"]);
        ExceptTest(ImmutableSortedSetValue.Create("a", "b", "c"), ["a", "b"], ["c"]);
        ExceptTest(ImmutableSortedSetValue.Create("a", "b", "c"), ImmutableSortedSetValue.Create("b", "d"), ["a", "c"]);

        static void ExceptTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Except(items);
            VerifyImmutableSortedSet(expected, actual);
        }
    }

    [TestMethod]
    public override void UnionTest()
    {
        UnionTest(default, [], default);
        UnionTest(default, Array.Empty<string>(), default);
        UnionTest(default, default(ImmutableSortedSetValue<string>), default);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableHashSetValue<string>), []);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        UnionTest(default, ["a", "b"], ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), ["a"], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), [], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), ["a"], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "b"), ["b", "c"], ["a", "b", "c"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "b"), ImmutableSortedSetValue.Create("a", "b"), ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "c", "d"), ImmutableSortedSetValue.Create("b", "c", "e"), ["a", "b", "c", "d", "e"]);

        static void UnionTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Union(items);
            VerifyImmutableSortedSet(expected, actual);
        }
    }

    [TestMethod]
    public override void IntersectTest()
    {
        IntersectTest(default, [], default);
        IntersectTest(default, Array.Empty<string>(), default);
        IntersectTest(default, default(ImmutableSortedSetValue<string>), default);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableHashSetValue<string>), []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        IntersectTest(default, ["a", "b"], default);
        IntersectTest(ImmutableSortedSetValue.Create("a"), [], []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), ["a"], []);
        IntersectTest(ImmutableSortedSetValue.Create("a"), ["a"], ["a"]);
        IntersectTest(ImmutableSortedSetValue.Create("a"), ["b"], []);
        IntersectTest(ImmutableSortedSetValue.Create("a", "c"), ["b", "c"], ["c"]);
        IntersectTest(ImmutableSortedSetValue.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["a", "c", "e"]);

        static void IntersectTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Intersect(items);
            VerifyImmutableSortedSet(expected, actual);
        }
    }

    [TestMethod]
    public override void SymmetricExceptTest()
    {
        SymmetricExceptTest(default, [], default);
        SymmetricExceptTest(default, Array.Empty<string>(), default);
        SymmetricExceptTest(default, default(ImmutableSortedSetValue<string>), default);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableHashSetValue<string>), []);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        SymmetricExceptTest(default, ["a", "b"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a"], ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), [], ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), ["a"], []);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a", "c"), ["b", "c"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["b", "d", "f", "g"]);

        static void SymmetricExceptTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.SymmetricExcept(items);
            VerifyImmutableSortedSet(expected, actual);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expectedItems">If <see langword="null"/>, <paramref name="actual"/> is expected to be <see langword="default"/></param>
    /// <param name="actual"></param>
    private static void VerifyImmutableSortedSet(List<string>? expectedItems, ImmutableSortedSetValue<string> actual)
    {
        if (expectedItems is null)
        {
            Assert.IsTrue(actual.IsDefault);
            Assert.AreEqual(0, actual.Count);
            Assert.AreEqual(0, actual.ToList().Count);
        }
        else
        {
            Assert.IsFalse(actual.IsDefault);
            Assert.AreEqual(expectedItems.Count, actual.Count);
            var actualItems = actual.ToList();
            CollectionAssert.AreEqual(expectedItems, actualItems);
        }
    }
}