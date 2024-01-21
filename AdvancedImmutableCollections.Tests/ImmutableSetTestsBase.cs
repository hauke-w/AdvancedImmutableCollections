using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public interface IImmutableSetWithEqualityComparerTests<TTestObject>
{
    TTestObject CreateInstance(GenericParameterHelper[] source, IEqualityComparer<GenericParameterHelper>? equalityComparer);
}

public abstract partial class ImmutableSetTestsBase<TTestObject, TMutable> : ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IImmutableSet<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
}

public abstract partial class ImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject, HashSet<GenericParameterHelper>>, IImmutableSetWithEqualityComparerTests<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override HashSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected abstract TTestObject CreateInstance(HashSet<GenericParameterHelper> source);

    protected override void AssertCollectionsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer = null)
        => CollectionAssert.That.AreEquivalent(expected, actual, itemComparer);

    TTestObject IImmutableSetWithEqualityComparerTests<TTestObject>.CreateInstance(GenericParameterHelper[] source, IEqualityComparer<GenericParameterHelper>? equalityComparer)
    {
        var hashSet = new HashSet<GenericParameterHelper>(source, equalityComparer);
        if (hashSet.Count != source.Length)
        {
            throw new ArgumentException("input items are not unique", nameof(source));
        }
        return CreateInstance(hashSet);
    }

    protected abstract IImmutableSet<GenericParameterHelper> Except(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> Union(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> Intersect(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> SymmetricExcept(TTestObject collection, IEnumerable<GenericParameterHelper> other);

#if NET6_0_OR_GREATER
    protected abstract override ISetEqualityWithEqualityComparerTestStrategy EqualityTestStrategy { get; }
#else
    protected sealed override IEqualityTestStrategy EqualityTestStrategy => SetEqualityTestStrategy;
    protected abstract ISetEqualityWithEqualityComparerTestStrategy SetEqualityTestStrategy { get; }
#endif

    [TestMethod]
    public void GetEnumeratorTest()
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

        if (DefaultValue is not null)
        {
            GetEnumeratorTestCore(DefaultValue, []);
        }

        void GetEnumeratorTest(GenericParameterHelper[] items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var itemsSet = new HashSet<GenericParameterHelper>(items, equalityComparer);
            if (itemsSet.Count != items.Length)
            {
                throw new ArgumentException("input items are not unique", nameof(items));
            }
            var testObject = CreateInstance(itemsSet);
            GetEnumeratorTestCore(testObject, itemsSet);
        }

        void GetEnumeratorTestCore(TTestObject testObject, HashSet<GenericParameterHelper> expected)
        {
            var actual = GetEnumerator(testObject);
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
    public void IEnumerable_GetEnumeratorTest()
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

        if (DefaultValue is not null)
        {
            GetEnumeratorTestCore(DefaultValue, []);
        }

        void GetEnumeratorTest(GenericParameterHelper[] items, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var itemsSet = new HashSet<GenericParameterHelper>(items, equalityComparer);
            if (itemsSet.Count != items.Length)
            {
                throw new ArgumentException("input items are not unique", nameof(items));
            }
            var testObject = CreateInstance(itemsSet);
            GetEnumeratorTestCore(testObject, itemsSet);
        }

        void GetEnumeratorTestCore(IEnumerable testObject, HashSet<GenericParameterHelper> expected)
        {
            IEnumerator actual = testObject.GetEnumerator();
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
#if NET6_0_OR_GREATER
        EqualityTestStrategy
#else
        SetEqualityTestStrategy
#endif
            .GetHashCode_ReferenceEqualityOfItems_Test(this);
    }

    [TestMethod]
    public void ExceptTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        ExceptTest([item0, item1, item2], [item1], [item0, item2], null);
        ExceptTest([item0, item1, item2, item3], [item0, item3], [item1, item2], ReferenceEqualityComparer.Instance);
        ExceptTest([item0, item1, item2], [item0, item1, item2, item3], [], EqualityComparer<GenericParameterHelper>.Default);
        ExceptTest([item0, item1, item2], [], [item0, item1, item2], null);
        ExceptTest([item0, item0b], [item1, item2, item3], [item0, item0b], ReferenceEqualityComparer.Instance);
        ExceptTest([item0, item1, item0b, item2], [item0b, item1, item2], [item0], ReferenceEqualityComparer.Instance);
        ExceptTest([item1b, item0, item1, item0b, item2], [item0, item3, item1b], [item1, item0b, item2], ReferenceEqualityComparer.Instance);
        ExceptTest([], [item0, item1], [], ReferenceEqualityComparer.Instance);
        ExceptTest([], [item0], [], ReferenceEqualityComparer.Instance);
        ExceptTest([], [], [], ReferenceEqualityComparer.Instance);

        if (DefaultValue is not null)
        {
            ExceptTestCore(DefaultValue, [], [], ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, [item0], [], ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, ImmutableArray.Create(item0), [], ReferenceEqualityComparer.Instance);
        }

        void ExceptTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            ExceptTestCore(testObject, otherItems, expected, equalityComparer);
        }

        void ExceptTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(Except, testObject, otherItems, expected, equalityComparer);
    }

    [TestMethod]
    public void UnionTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        UnionTest([], [], [], null);
        UnionTest([item0], [], [item0], null);
        UnionTest([], [item0], [item0], null);
        UnionTest([item0], [item0], [item0], null);
        UnionTest([item0], [item0b], [item0], EqualityComparer<GenericParameterHelper>.Default);
        UnionTest([item0], [item0b], [item0, item0b], ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1, item1b, item3], [item0, item0b, item1, item1b, item2, item3], ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1b, item3], [item0, item1, item2, item3], null);

        if (DefaultValue is not null)
        {
            UnionTestCore(DefaultValue, [], [], ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, [item0], [item0], ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, ImmutableArray.Create(item0), [item0], ReferenceEqualityComparer.Instance);
        }

        void UnionTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            UnionTestCore(testObject, otherItems, expected, equalityComparer);
        }

        void UnionTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(Union, testObject, otherItems, expected, equalityComparer);
    }

    private void VerifySetOperation(Func<TTestObject, IEnumerable<GenericParameterHelper>, IImmutableSet<GenericParameterHelper>> operation, TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
    {
        bool expectChange = expected.Length != testObject.Count;
        var initialItems = testObject.ToList();
        var other = new HashSet<GenericParameterHelper>(otherItems, equalityComparer);
        var actual = operation(testObject, other);
        AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
        AssertCollectionsAreEqual(initialItems, testObject, ReferenceEqualityComparer.Instance);

        if (expectChange)
        {
            Assert.AreNotSame(testObject, actual);
        }
        else if (DefaultValue is not null)
        {
            // it's a value type so Assert.AreSame cannot be used
            Assert.AreEqual(testObject, actual);
        }
        else
        {
            Assert.AreSame(testObject, actual);
        }
    }
}
