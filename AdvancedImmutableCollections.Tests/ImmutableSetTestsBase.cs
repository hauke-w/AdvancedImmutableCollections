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
    where TMutable :  ICollection<GenericParameterHelper>
{
}

public abstract partial class ImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject, HashSet<GenericParameterHelper>>, IImmutableSetWithEqualityComparerTests<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override HashSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected abstract TTestObject CreateInstance(HashSet<GenericParameterHelper> source);

    TTestObject IImmutableSetWithEqualityComparerTests<TTestObject>.CreateInstance(GenericParameterHelper[] source, IEqualityComparer<GenericParameterHelper>? equalityComparer)
    {
        var hashSet = new HashSet<GenericParameterHelper>(source, equalityComparer);
        if (hashSet.Count != source.Length)
        {
            throw new ArgumentException("input items are not unique", nameof(source));
        }
        return CreateInstance(hashSet);
    }

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
}
