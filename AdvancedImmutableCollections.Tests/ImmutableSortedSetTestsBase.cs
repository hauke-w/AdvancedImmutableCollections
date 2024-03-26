using System.Collections;
using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.CollectionAdapters;

namespace AdvancedImmutableCollections;

public abstract class ImmutableSortedSetTestsBase<TFactory> : ImmutableSetTestsBase<TFactory>
    where TFactory : IImmutableSortedSetAdapterFactory, new()
{
    protected override IEqualityTestStrategy EqualityTestStrategy => new SortedSetValueEqualityTestStrategy();

    private class SortedSetValueEqualityTestStrategy : ValueEqualityTestStrategy
    {
        public SortedSetValueEqualityTestStrategy()
        {
            DefaultValueIsEqualEmpty = true;
        }
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
        ExceptTest(default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), default);
        ExceptTest(default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), default);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableArray<string>), []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a"], []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a", "b"], []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, ["a"]), []);
        ExceptTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        ExceptTest(ImmutableSortedSetValue.Create("a"), ["a"], []);
        ExceptTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a"]);
        ExceptTest(ImmutableSortedSetValue.Create("a", "b", "c"), ["a", "b"], ["c"]);
        ExceptTest(ImmutableSortedSetValue.Create("a", "b", "c"), ImmutableSortedSetValue.Create("b", "d"), ["a", "c"]);

        ExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C"), ImmutableSortedSetValue.Create("b", "c"), ["a"]);
        ExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b", "c", "C", "d", "D"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "b", "c", "D"), ["a", "C", "d"]);
        ExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c", "d", "E", "F"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "C", "d", "D", "e", "E"), ["a", "F"]);

        static void ExceptTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Except(items);
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void UnionTest()
    {
        UnionTest(default, [], default);
        UnionTest(default, Array.Empty<string>(), default);
        UnionTest(default, default(ImmutableSortedSetValue<string>), default);
        UnionTest(default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), ["a", "b"]);
        UnionTest(default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), ["a"]);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), []);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableSortedSetValue<string>), []);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        UnionTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCultureIgnoreCase), default(ImmutableSortedSetValue<string>), []);
        UnionTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), ImmutableSortedSetValue.Create<string>("a"), ["a"]);
        UnionTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), default(ImmutableSortedSetValue<string>), []);
        UnionTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSetValue.Create<string>("a"), ["a"]);
        UnionTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSet.Create<string>("a"), ["a"]);
        UnionTest(default, ["a", "b"], ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Empty<string>(), ["a"], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), [], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), ["a"], ["a"]);
        UnionTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "b"), ["b", "c"], ["a", "b", "c"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "b"), ImmutableSortedSetValue.Create("a", "b"), ["a", "b"]);
        UnionTest(ImmutableSortedSetValue.Create("a", "c", "d"), ImmutableSortedSetValue.Create("b", "c", "e"), ["a", "b", "c", "d", "e"]);

        UnionTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C"), ImmutableSortedSetValue.Create("B", "c", "d"), ["a", "b", "C", "d"]);
        UnionTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b", "C"), ImmutableSortedSetValue.Create("B", "c", "d"), ["a", "b", "B", "c", "C", "d"]);
        UnionTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "A", "B", "d", "D", "E"), ["a", "B", "c", "d", "E"]);

        static void UnionTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Union(items);
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void IntersectTest()
    {
        IntersectTest(default, [], default);
        IntersectTest(default, Array.Empty<string>(), default);
        IntersectTest(default, default(ImmutableSortedSetValue<string>), default);
        IntersectTest(default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), default);
        IntersectTest(default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), default);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableSortedSetValue<string>), []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        IntersectTest(default, ["a", "b"], default);
        IntersectTest(ImmutableSortedSetValue.Create("a"), [], []);
        IntersectTest(ImmutableSortedSetValue.Empty<string>(), ["a"], []);
        IntersectTest(ImmutableSortedSetValue.Create("a"), ["a"], ["a"]);
        IntersectTest(ImmutableSortedSetValue.Create("a"), ["b"], []);
        IntersectTest(ImmutableSortedSetValue.Create("a", "c"), ["b", "c"], ["c"]);
        IntersectTest(ImmutableSortedSetValue.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["a", "c", "e"]);

        IntersectTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C", "d", "E", "G"), ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "b", "c", "D", "F", "g", "G"), ["b", "c", "D", "g"]);
        IntersectTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "A", "b", "B", "C", "d", "F"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "A", "b", "c", "D", "e"), ["A", "b"]);
        IntersectTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "c"), ["b", "c"]);

        static void IntersectTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.Intersect(items);
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void SymmetricExceptTest()
    {
        SymmetricExceptTest(default, [], default);
        SymmetricExceptTest(default, Array.Empty<string>(), default);
        SymmetricExceptTest(default, default(ImmutableSortedSetValue<string>), default);
        SymmetricExceptTest(default, ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), default);
        SymmetricExceptTest(default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), ["a"]);
        SymmetricExceptTest(default, ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), default(ImmutableSortedSetValue<string>), []);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), Array.Empty<string>(), []);
        SymmetricExceptTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), default(ImmutableSortedSetValue<string>), []);
        SymmetricExceptTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSetValue.Create("a"), ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSet.Create("a"), ["a"]);
        SymmetricExceptTest(default, ["a", "b"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Empty<string>(), ["a"], ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), [], ["a"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), ["a"], []);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a"), ["b"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a", "c"), ["b", "c"], ["a", "b"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["b", "d", "f", "g"]);

        SymmetricExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c", "E"), ImmutableSortedSetValue.Create("b", "c", "D"), ["a", "D", "E"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "A", "b", "B", "C", "d", "F"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "A", "b", "c", "e"), ["a", "B", "c", "C", "d", "e", "F"]);
        SymmetricExceptTest(ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "B", "C", "d"), ["a", "d"]);

        void SymmetricExceptTest(ImmutableSortedSetValue<string> testObject, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObject.SymmetricExcept(items);
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expectedItems">If <see langword="null"/>, <paramref name="actual"/> is expected to be <see langword="default"/></param>
    /// <param name="actual"></param>
    private static void VerifyImmutableSortedSet(List<string>? expectedItems, IComparer<string> expectedComparer, ImmutableSortedSetValue<string> actual)
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
            Assert.AreEqual(expectedComparer, actual.Value.KeyComparer);
        }
    }
}
