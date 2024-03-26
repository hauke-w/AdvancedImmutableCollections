using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableHashSetTestsBase<TFactory> : ImmutableSetTestsBase<TFactory>
    where TFactory : IImmutableSetWithEqualityComparerAdapterFactory, new()
{
    protected override void AssertCollectionsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer = null)
        => CollectionAssert.That.AreEquivalent(expected, actual, itemComparer);

    protected override IEqualityTestStrategy EqualityTestStrategy
        => SetValueEqualityTestStrategy.Default;

    [TestMethod]
    public sealed override void GetEnumeratorTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        GetEnumeratorTest([], ReferenceEqualityComparer.Instance);
        GetEnumeratorTest([], null);
        GetEnumeratorTest([item0], EqualityComparer<GenericParameterHelper>.Default);
        GetEnumeratorTest([item0, item0b], ReferenceEqualityComparer.Instance);
        GetEnumeratorTest([item0b, item2, item0, item1, item1b], ReferenceEqualityComparer.Instance);
        GetEnumeratorTest([item0, item1, item2], null);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            GetEnumeratorTestCore(@default, []);
        }

        void GetEnumeratorTest(GenericParameterHelper[] items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var itemsSet = new HashSet<GenericParameterHelper>(items, equalityComparer);
            if (itemsSet.Count != items.Length)
            {
                throw new ArgumentException("input items are not unique", nameof(items));
            }
            var testObjectAdapter = Factory.Create(equalityComparer, items);
            GetEnumeratorTestCore(testObjectAdapter, itemsSet);
        }

        void GetEnumeratorTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, HashSet<GenericParameterHelper> expected)
        {
            var actual = testObjectAdapter.GetEnumerator();
            Assert.IsNotNull(actual);

            int actualCount = 0;
            while (actualCount < expected.Count)
            {
                Assert.IsTrue(actual.MoveNext(), "less elements than expected");
                actualCount++;
                Assert.IsTrue(expected.Contains(actual.Current));
            }

            Assert.IsFalse(actual.MoveNext(), "more elements than expected");
        }
    }

    [TestMethod]
    public sealed override void IEnumerable_GetEnumeratorTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        GetEnumeratorTest([], EqualityComparer<GenericParameterHelper>.Default);
        GetEnumeratorTest([item0], null);
        GetEnumeratorTest([item0, item0b], ReferenceEqualityComparer.Instance);
        GetEnumeratorTest([item0, item1, item2, item1b, item0b], ReferenceEqualityComparer.Instance);
        GetEnumeratorTest([item0, item1, item2], EqualityComparer<GenericParameterHelper>.Default);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            GetEnumeratorTestCore(@default, []);
        }

        void GetEnumeratorTest(GenericParameterHelper[] items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var itemsSet = new HashSet<GenericParameterHelper>(items, equalityComparer);
            var testObjectAdapter = Factory.Create(equalityComparer, items);
            if (testObjectAdapter.Count != items.Length)
            {
                throw new ArgumentException("input items are not unique", nameof(items));
            }
            GetEnumeratorTestCore(testObjectAdapter, itemsSet);
        }

        void GetEnumeratorTestCore(IEnumerable testObjectAdapter, HashSet<GenericParameterHelper> expected)
        {
            IEnumerator actual = testObjectAdapter.GetEnumerator();
            Assert.IsNotNull(actual);

            int actualCount = 0;
            while (actualCount < expected.Count)
            {
                Assert.IsTrue(actual.MoveNext(), "less elements than expected");
                actualCount++;
                Assert.IsTrue(expected.Contains(actual.Current));
            }

            Assert.IsFalse(actual.MoveNext(), "more elements than expected");
        }
    }

    [TestMethod]
    public void GetHashCode_Duplicates_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item2b = new GenericParameterHelper(2);

        var items0 = CreateInstance(item0);
        var items01 = CreateInstance(item0, item1);

        AssertEqualHashCode(items0, CreateInstance(item0b));
        AssertEqualHashCode(items01, CreateInstance(item0b, item1));
        AssertEqualHashCode(items01, CreateInstance(item0, item1b));
        AssertEqualHashCode(items01, CreateInstance(item0b, item1b));

        var hashcodes = new HashSet<int>();
        IImmutableSetAdapter<GenericParameterHelper>[] valuesWithUniqueHashcode =
            [
                CreateInstance(),
                    items0,
                    items01,
                    CreateInstance(item0, item1, item2),
                    CreateInstance(item0, item0b),
                    CreateInstance(item0, item0b, item1),
                    CreateInstance(item0, item0b, item1, item1b),
                    CreateInstance(item0, item0b, item1, item1b, item2),
                    CreateInstance(item0, item0b, item1, item1b, item2, item2b),
                ];

        foreach (var testObject in valuesWithUniqueHashcode)
        {
            var actual = testObject.GetHashCode();
            Assert.IsTrue(hashcodes.Add(actual), "hash code is not unique");
        }

        IImmutableSetAdapter<GenericParameterHelper> CreateInstance(params GenericParameterHelper[] items)
            => Factory.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, items);

        void AssertEqualHashCode(IImmutableSetAdapter<GenericParameterHelper> items1, IImmutableSetAdapter<GenericParameterHelper> items2)
        {
            var hashcode1 = items1.GetHashCode();
            var hashcode2 = items2.GetHashCode();
            Assert.AreEqual(hashcode1, hashcode2);
        }
    }

    [TestMethod]
    public sealed override void ExceptTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        ExceptTest([item0, item1, item2], [item1], [item0, item2], true, null);
        ExceptTest([item0, item1, item2, item3], [item0, item3], [item1, item2], true, ReferenceEqualityComparer.Instance);
        ExceptTest([item0, item1, item2], [item0, item1, item2, item3], [], true, EqualityComparer<GenericParameterHelper>.Default);
        ExceptTest([item0, item1, item2], [], [item0, item1, item2], false, null);
        ExceptTest([item0, item0b], [item1, item2, item3], [item0, item0b], false, ReferenceEqualityComparer.Instance);
        ExceptTest([item0, item1, item0b, item2], [item0b, item1, item2], [item0], true, ReferenceEqualityComparer.Instance);
        ExceptTest([item1b, item0, item1, item0b, item2], [item0, item3, item1b], [item1, item0b, item2], true, ReferenceEqualityComparer.Instance);
        ExceptTest([], [item0, item1], [], false, ReferenceEqualityComparer.Instance);
        ExceptTest([], [item0], [], false, ReferenceEqualityComparer.Instance);
        ExceptTest([], [], [], false, ReferenceEqualityComparer.Instance);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            ExceptTestCore(@default, [], [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(@default, [item0], [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(@default, new HashSet<GenericParameterHelper>(), [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(@default, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(@default, ImmutableArray.Create(item0), [], false, ReferenceEqualityComparer.Instance);
        }

        void ExceptTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var testObjectAdapter = Factory.Create(equalityComparer, testObjectItems);
            ExceptTestCore(testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void ExceptTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(static (a, b) => a.Except(b), testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparer);
    }

    [TestMethod]
    public sealed override void UnionTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        UnionTest([], [], [], false, null);
        UnionTest([item0], [], [item0], false, null);
        UnionTest([], [item0], [item0], true, null);
        UnionTest([item0], [item0], [item0], false, null);
        UnionTest([item0], [item0b], [item0], false, EqualityComparer<GenericParameterHelper>.Default);
        UnionTest([item0], [item0b], [item0, item0b], true, ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1, item1b, item3], [item0, item0b, item1, item1b, item2, item3], true, ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1b, item3], [item0, item1, item2, item3], true, null);

        UnionTest([], new ImmutableHashSetValue<GenericParameterHelper>(ReferenceEqualityComparer.Instance, [item0, item0b]), [item0], true, EqualityComparer<GenericParameterHelper>.Default);
        UnionTest([], ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b }), [item0, item1], true, EqualityComparer<GenericParameterHelper>.Default);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            UnionTestCore(@default, [], [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(@default, [item0], [item0], true, ReferenceEqualityComparer.Instance);
            UnionTestCore(@default, new HashSet<GenericParameterHelper>(), [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(@default, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(@default, ImmutableArray.Create(item0), [item0], true, ReferenceEqualityComparer.Instance);
            UnionTestCore(@default, new ImmutableHashSetValue<GenericParameterHelper>(ReferenceEqualityComparer.Instance, [item0, item0b]), [item0], true, EqualityComparer<GenericParameterHelper>.Default);
            UnionTestCore(@default, ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b }), [item0, item1], true, EqualityComparer<GenericParameterHelper>.Default);
        }

        void UnionTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var testObjectAdapter = Factory.Create(equalityComparer, testObjectItems);
            UnionTestCore(testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void UnionTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparerForVerification)
            => VerifySetOperation(static (a, b) => a.Union(b), testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparerForVerification);
    }

    [TestMethod]
    public sealed override void IntersectTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);

        IntersectTest([], [], [], false, null);
        IntersectTest([], [], [], false, ReferenceEqualityComparer.Instance);
        IntersectTest([item0], [], [], true, null);
        IntersectTest([], [item0], [], false, null);
        IntersectTest([item0], [item0b], [item0], false, EqualityComparer<GenericParameterHelper>.Default);
        IntersectTest([item0], [item0b], [], true, ReferenceEqualityComparer.Instance);
        IntersectTest([item0, item1, item2, item3], [item0b, item1b], [item0, item1], true, EqualityComparer<GenericParameterHelper>.Default);
        IntersectTest([item6, item7, item0, item1, item2, item3], [item0b, item4, item5, item1b], [item0, item1], true, null);
        IntersectTest([item0, item1, item2, item3], [item0b, item1b], [], true, ReferenceEqualityComparer.Instance);
        IntersectTest([item0, item1, item0b, item2, item3, item1b], [item4, item0b, item1b, item5, item1, item6], [item1, item0b, item1b], true, ReferenceEqualityComparer.Instance);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            IntersectTestCore(@default, [], [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(@default, [item0], [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(@default, new HashSet<GenericParameterHelper>() { item1 }, [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(@default, new HashSet<GenericParameterHelper>(), [], false, EqualityComparer<GenericParameterHelper>.Default);
            IntersectTestCore(@default, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(@default, ImmutableArray.Create(item2), [], false, ReferenceEqualityComparer.Instance);
        }

        void IntersectTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var testObjectAdapter = Factory.Create(equalityComparer, testObjectItems);
            var equalityComparerForVerification = VerifyIntersectWithReferenceEquality ? equalityComparer : EqualityComparer<GenericParameterHelper>.Default;
            IntersectTestCore(testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void IntersectTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparerForVerification)
            => VerifySetOperation(static (a, b) => a.Intersect(b), testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparerForVerification);
    }

    public virtual bool VerifyIntersectWithReferenceEquality => true;

    [TestMethod]
    public sealed override void SymmetricExceptTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);

        SymmetricExceptTest([], [], [], false, null);
        SymmetricExceptTest([], [], [], false, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0], [item0], [], true, null);
        SymmetricExceptTest([], [item0], [item0], true, EqualityComparer<GenericParameterHelper>.Default);
        SymmetricExceptTest([item0], [item0b], [], true, EqualityComparer<GenericParameterHelper>.Default);
        SymmetricExceptTest([item0], [item0b], [item0, item0b], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0, item1], [item0b], [item1], true, EqualityComparer<GenericParameterHelper>.Default);
        SymmetricExceptTest([item0], [item2, item0b], [item2], true, EqualityComparer<GenericParameterHelper>.Default);
        SymmetricExceptTest([item0, item1], [item0b, item2], [item1, item2], true, EqualityComparer<GenericParameterHelper>.Default);
        SymmetricExceptTest([item0], [item0b], [item0, item0b], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0, item0b], [item0b], [item0], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0], [item0b, item0], [item0b], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0, item1, item0b, item2, item3, item1b], [item4, item0b, item1b, item5, item1, item6], [item0, item2, item3, item4, item5, item6], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([item0, item1, item0b, item2, item3, item1b], [], [item0, item1, item0b, item2, item3, item1b], false, ReferenceEqualityComparer.Instance);

        SymmetricExceptTest([], ImmutableArray<GenericParameterHelper>.Empty, [], false, null);
        SymmetricExceptTest([], ImmutableArray.Create(item0, item1, item0b, item2, item1b), [item0, item1, item2], true, null);
        SymmetricExceptTest([], ImmutableHashSet.Create(item0, item1, item0b, item2, item1b), [item0, item1, item2], true, null);
        SymmetricExceptTest([], ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b, item2, item1b }), [item0, item1, item2], true, null);
        SymmetricExceptTest([], new GenericParameterHelper[] { item0, item1, item0b, item2, item1b }.WrapAsEnumerable(), [item0, item1, item0b, item2, item1b], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([], ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new[] { item0, item1, item1b, item2 }).WithValueSemantics(), [item0, item1, item2], true, null);
        SymmetricExceptTest([], new HashSet<GenericParameterHelper>([item0, item1]), [item0, item1], true, ReferenceEqualityComparer.Instance);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            SymmetricExceptTestCore(@default, [], [], false, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(@default, [item0], [item0], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(@default, new HashSet<GenericParameterHelper>() { item1 }, [item1], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(@default, new HashSet<GenericParameterHelper>(), [], false, EqualityComparer<GenericParameterHelper>.Default);
            SymmetricExceptTestCore(@default, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(@default, ImmutableArray.Create(item2), [item2], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(@default, ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b }), [item0, item1], true);

            SymmetricExceptTestCore(@default, ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new[] { item0, item0b }).WithValueSemantics(), [item0], true, null);
        }

        void SymmetricExceptTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var testObjectAdapter = Factory.Create(equalityComparer, testObjectItems);
            SymmetricExceptTestCore(testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void SymmetricExceptTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparerForVerification = null)
            => VerifySetOperation(static (a, b) => a.SymmetricExcept(b), testObjectAdapter, otherItems, expected, isChangeExpected, equalityComparerForVerification);
    }
}
