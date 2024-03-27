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
        var @default = Factory.GetDefaultValue<string>();
        Assert.IsNotNull(@default);
        ExceptTest(@default, Array.Empty<string>(), default);
        ExceptTest(@default, default(ImmutableSortedSetValue<string>), default);
        ExceptTest(@default, ["a"], default);
        ExceptTest(@default, ImmutableSortedSetValue.Create("a", "b"), default);
        ExceptTest(@default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), default);
        ExceptTest(@default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), default);
        ExceptTest(Factory.Create<string>(), default(ImmutableArray<string>), []);
        ExceptTest(Factory.Create<string>(), ["a"], []);
        ExceptTest(Factory.Create<string>(), ["a", "b"], []);
        ExceptTest(Factory.Create<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, ["a"]), []);
        ExceptTest(Factory.Create<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        ExceptTest(Factory.Create("a"), ["a"], []);
        ExceptTest(Factory.Create("a"), ["b"], ["a"]);
        ExceptTest(Factory.Create("a", "b", "c"), ["a", "b"], ["c"]);
        ExceptTest(Factory.Create("a", "b", "c"), ImmutableSortedSetValue.Create("b", "d"), ["a", "c"]);

        ExceptTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C"), ImmutableSortedSetValue.Create("b", "c"), ["a"]);
        ExceptTest(Factory.Create(StringComparer.InvariantCulture, "a", "b", "c", "C", "d", "D"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "b", "c", "D"), ["a", "C", "d"]);
        ExceptTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c", "d", "E", "F"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "C", "d", "D", "e", "E"), ["a", "F"]);

        static void ExceptTest(IImmutableSetAdapter<string> testObjectAdapter, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObjectAdapter.Except(items);
            var testObject = (ImmutableSortedSetValue<string>)testObjectAdapter.Collection;
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void UnionTest()
    {
        var @default = Factory.GetDefaultValue<string>();
        Assert.IsNotNull(@default);
        UnionTest(@default, [], default);
        UnionTest(@default, Array.Empty<string>(), default);
        UnionTest(@default, default(ImmutableSortedSetValue<string>), default);
        UnionTest(@default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), ["a", "b"]);
        UnionTest(@default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), ["a"]);
        UnionTest(Factory.Create<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), []);
        UnionTest(Factory.Create<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        UnionTest(Factory.Create<string>(), default(ImmutableSortedSetValue<string>), []);
        UnionTest(Factory.Create<string>(), Array.Empty<string>(), []);
        UnionTest(Factory.Create<string>(StringComparer.InvariantCultureIgnoreCase), default(ImmutableSortedSetValue<string>), []);
        UnionTest(Factory.Create<string>(StringComparer.InvariantCulture), ImmutableSortedSetValue.Create<string>("a"), ["a"]);
        UnionTest(Factory.Create<string>(StringComparer.InvariantCulture), default(ImmutableSortedSetValue<string>), []);
        UnionTest(Factory.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSetValue.Create<string>("a"), ["a"]);
        UnionTest(Factory.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSet.Create<string>("a"), ["a"]);
        UnionTest(@default, ["a", "b"], ["a", "b"]);
        UnionTest(Factory.Create<string>(), ["a"], ["a"]);
        UnionTest(Factory.Create("a"), [], ["a"]);
        UnionTest(Factory.Create("a"), ["a"], ["a"]);
        UnionTest(Factory.Create("a"), ["b"], ["a", "b"]);
        UnionTest(Factory.Create("a", "b"), ["b", "c"], ["a", "b", "c"]);
        UnionTest(Factory.Create("a", "b"), ImmutableSortedSetValue.Create("a", "b"), ["a", "b"]);
        UnionTest(Factory.Create("a", "c", "d"), ImmutableSortedSetValue.Create("b", "c", "e"), ["a", "b", "c", "d", "e"]);

        UnionTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C"), ImmutableSortedSetValue.Create("B", "c", "d"), ["a", "b", "C", "d"]);
        UnionTest(Factory.Create(StringComparer.InvariantCulture, "a", "b", "C"), ImmutableSortedSetValue.Create("B", "c", "d"), ["a", "b", "B", "c", "C", "d"]);
        UnionTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "A", "B", "d", "D", "E"), ["a", "B", "c", "d", "E"]);

        static void UnionTest(IImmutableSetAdapter<string> testObjectAdapter, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObjectAdapter.Union(items);
            var testObject = (ImmutableSortedSetValue<string>)testObjectAdapter.Collection;
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void IntersectTest()
    {
        var @default = Factory.GetDefaultValue<string>();
        Assert.IsNotNull(@default);
        IntersectTest(@default, [], default);
        IntersectTest(@default, Array.Empty<string>(), default);
        IntersectTest(@default, default(ImmutableSortedSetValue<string>), default);
        IntersectTest(@default, ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "a", "b"), default);
        IntersectTest(@default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), default);
        IntersectTest(Factory.Create<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), []);
        IntersectTest(Factory.Create<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        IntersectTest(Factory.Create<string>(), default(ImmutableSortedSetValue<string>), []);
        IntersectTest(Factory.Create<string>(), Array.Empty<string>(), []);
        IntersectTest(@default, ["a", "b"], default);
        IntersectTest(Factory.Create("a"), [], []);
        IntersectTest(Factory.Create<string>(), ["a"], []);
        IntersectTest(Factory.Create("a"), ["a"], ["a"]);
        IntersectTest(Factory.Create("a"), ["b"], []);
        IntersectTest(Factory.Create("a", "c"), ["b", "c"], ["c"]);
        IntersectTest(Factory.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["a", "c", "e"]);

        IntersectTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "b", "C", "d", "E", "G"), ImmutableSortedSetValue.Create(StringComparer.InvariantCulture, "b", "c", "D", "F", "g", "G"), ["b", "c", "D", "g"]);
        IntersectTest(Factory.Create(StringComparer.InvariantCulture, "a", "A", "b", "B", "C", "d", "F"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "A", "b", "c", "D", "e"), ["A", "b"]);
        IntersectTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "c"), ["b", "c"]);

        static void IntersectTest(IImmutableSetAdapter<string> testObjectAdapter, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObjectAdapter.Intersect(items);
            var testObject = (ImmutableSortedSetValue<string>)testObjectAdapter.Collection;
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    [TestMethod]
    public override void SymmetricExceptTest()
    {
        var @default = Factory.GetDefaultValue<string>();
        Assert.IsNotNull(@default);
        SymmetricExceptTest(@default, [], default);
        SymmetricExceptTest(@default, Array.Empty<string>(), default);
        SymmetricExceptTest(@default, default(ImmutableSortedSetValue<string>), default);
        SymmetricExceptTest(@default, ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture), default);
        SymmetricExceptTest(@default, ImmutableSortedSet.Create(StringComparer.InvariantCultureIgnoreCase, "a"), ["a"]);
        SymmetricExceptTest(@default, ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), ["a"]);
        SymmetricExceptTest(Factory.Create<string>(), ImmutableSortedSetValue.Create<string>(StringComparer.InvariantCulture, "a"), ["a"]);
        SymmetricExceptTest(Factory.Create<string>(), ImmutableSortedSet.Create<string>(StringComparer.InvariantCultureIgnoreCase), []);
        SymmetricExceptTest(Factory.Create<string>(), default(ImmutableSortedSetValue<string>), []);
        SymmetricExceptTest(Factory.Create<string>(), Array.Empty<string>(), []);
        SymmetricExceptTest(Factory.Create<string>(StringComparer.InvariantCulture), default(ImmutableSortedSetValue<string>), []);
        SymmetricExceptTest(Factory.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSetValue.Create("a"), ["a"]);
        SymmetricExceptTest(Factory.Create<string>(StringComparer.InvariantCultureIgnoreCase), ImmutableSortedSet.Create("a"), ["a"]);
        SymmetricExceptTest(@default, ["a", "b"], ["a", "b"]);
        SymmetricExceptTest(Factory.Create<string>(), ["a"], ["a"]);
        SymmetricExceptTest(Factory.Create("a"), [], ["a"]);
        SymmetricExceptTest(Factory.Create("a"), ["a"], []);
        SymmetricExceptTest(Factory.Create("a"), ["b"], ["a", "b"]);
        SymmetricExceptTest(Factory.Create("a", "c"), ["b", "c"], ["a", "b"]);
        SymmetricExceptTest(Factory.Create("a", "c", "d", "e", "g"), ImmutableSortedSetValue.Create("a", "b", "c", "e", "f"), ["b", "d", "f", "g"]);

        SymmetricExceptTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c", "E"), ImmutableSortedSetValue.Create("b", "c", "D"), ["a", "D", "E"]);
        SymmetricExceptTest(Factory.Create(StringComparer.InvariantCulture, "a", "A", "b", "B", "C", "d", "F"), ImmutableSortedSetValue.Create(StringComparer.InvariantCultureIgnoreCase, "A", "b", "c", "e"), ["a", "B", "c", "C", "d", "e", "F"]);
        SymmetricExceptTest(Factory.Create(StringComparer.InvariantCultureIgnoreCase, "a", "B", "c"), ImmutableSortedSet.Create(StringComparer.InvariantCulture, "b", "B", "C", "d"), ["a", "d"]);

        void SymmetricExceptTest(IImmutableSetAdapter<string> testObjectAdapter, IEnumerable<string> items, List<string>? expected)
        {
            var actual = testObjectAdapter.SymmetricExcept(items);
            var testObject = (ImmutableSortedSetValue<string>)testObjectAdapter.Collection;
            VerifyImmutableSortedSet(expected, testObject.Value.KeyComparer, actual);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="expectedItems">If <see langword="null"/>, <paramref name="actual"/> is expected to be <see langword="default"/></param>
    /// <param name="actual"></param>
    private static void VerifyImmutableSortedSet(List<string>? expectedItems, IComparer<string> expectedComparer, IImmutableSet<string> actualValue)
    {
        Assert.IsInstanceOfType(actualValue, typeof(ImmutableSortedSetValue<string>));
        var actual = (ImmutableSortedSetValue<string>)actualValue;
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
