﻿using AdvancedImmutableCollections.Tests.Util;
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

        if (DefaultValue is not null)
        {
            ExceptTestCore(DefaultValue, [], [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, [item0], [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            ExceptTestCore(DefaultValue, ImmutableArray.Create(item0), [], false, ReferenceEqualityComparer.Instance);
        }

        void ExceptTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            ExceptTestCore(testObject, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void ExceptTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(Except, testObject, otherItems, expected, isChangeExpected, equalityComparer);
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

        UnionTest([], [], [], false, null);
        UnionTest([item0], [], [item0], false, null);
        UnionTest([], [item0], [item0], true, null);
        UnionTest([item0], [item0], [item0], false, null);
        UnionTest([item0], [item0b], [item0], false, EqualityComparer<GenericParameterHelper>.Default);
        UnionTest([item0], [item0b], [item0, item0b], true, ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1, item1b, item3], [item0, item0b, item1, item1b, item2, item3], true, ReferenceEqualityComparer.Instance);
        UnionTest([item0, item1, item2], [item0b, item1b, item3], [item0, item1, item2, item3], true, null);

        if (DefaultValue is not null)
        {
            UnionTestCore(DefaultValue, [], [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, [item0], [item0], true, ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            UnionTestCore(DefaultValue, ImmutableArray.Create(item0), [item0], true, ReferenceEqualityComparer.Instance);
        }

        void UnionTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            UnionTestCore(testObject, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void UnionTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(Union, testObject, otherItems, expected, isChangeExpected, equalityComparer);
    }

    [TestMethod]
    public void IntersectTest()
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

        if (DefaultValue is not null)
        {
            IntersectTestCore(DefaultValue, [], [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(DefaultValue, [item0], [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(DefaultValue, new HashSet<GenericParameterHelper>() { item1 }, [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], false, EqualityComparer<GenericParameterHelper>.Default);
            IntersectTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            IntersectTestCore(DefaultValue, ImmutableArray.Create(item2), [], false, ReferenceEqualityComparer.Instance);
        }

        void IntersectTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            IntersectTestCore(testObject, otherItems, expected, isChangeExpected, equalityComparer);
        }

        void IntersectTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer)
            => VerifySetOperation(Intersect, testObject, otherItems, expected, isChangeExpected, equalityComparer, checkReferenceEquality: VerifyIntersectWithReferenceEquality);
    }

    public virtual bool VerifyIntersectWithReferenceEquality => true;

    [TestMethod]
    public void SymmetricExceptTest()
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
        SymmetricExceptTest([], ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b, item2, item1b }), [item0, item1, item2], true, null, checkReferenceEquality: false); // checkReferenceEquality: false because the actual result instances are not deterministic
        SymmetricExceptTest([], new GenericParameterHelper[] { item0, item1, item0b, item2, item1b }.WrapAsEnumerable(), [item0, item1, item0b, item2, item1b], true, ReferenceEqualityComparer.Instance);
        SymmetricExceptTest([], ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new[] { item0, item1, item1b, item2 }).WithValueSemantics(), [item0, item1, item2], true, null, checkReferenceEquality: false);
        SymmetricExceptTest([], new HashSet<GenericParameterHelper>([item0, item1]), [item0, item1], true, ReferenceEqualityComparer.Instance);

        if (DefaultValue is not null)
        {
            SymmetricExceptTestCore(DefaultValue, [], [], false, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(DefaultValue, [item0], [item0], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(DefaultValue, new HashSet<GenericParameterHelper>() { item1 }, [item1], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(DefaultValue, new HashSet<GenericParameterHelper>(), [], false, EqualityComparer<GenericParameterHelper>.Default);
            SymmetricExceptTestCore(DefaultValue, ImmutableArray<GenericParameterHelper>.Empty, [], false, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(DefaultValue, ImmutableArray.Create(item2), [item2], true, ReferenceEqualityComparer.Instance);
            SymmetricExceptTestCore(DefaultValue, ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new GenericParameterHelper[] { item0, item1, item0b }), [item0, item1], true, null, checkReferenceEquality: false); // checkReferenceEquality: false because the actual result instances are not deterministic

            SymmetricExceptTestCore(DefaultValue, ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, new[] { item0, item0b }).WithValueSemantics(), [item0], true, null, checkReferenceEquality: false);
        }

        void SymmetricExceptTest(GenericParameterHelper[] testObjectItems, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer, bool checkReferenceEquality = true)
        {
            var initialItems = new HashSet<GenericParameterHelper>(testObjectItems, equalityComparer);
            var testObject = CreateInstance(initialItems);
            SymmetricExceptTestCore(testObject, otherItems, expected, isChangeExpected, equalityComparer, checkReferenceEquality);
        }

        void SymmetricExceptTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> otherItems, GenericParameterHelper[] expected, bool isChangeExpected, IEqualityComparer<GenericParameterHelper>? equalityComparer, bool checkReferenceEquality = true)
            => VerifySetOperation(SymmetricExcept, testObject, otherItems, expected, isChangeExpected, equalityComparer, checkReferenceEquality);
    }

    /// <summary>
    /// A function that will produce a manipulated set from the <paramref name="set"/> and <paramref name="other"/> parameter.
    /// </summary>
    /// <param name="set">The set on which the operation is executed</param>
    /// <param name="other">parameter of the operation that will be used to compute the result</param>
    /// <returns></returns>
    protected delegate IImmutableSet<GenericParameterHelper> SetOperation(TTestObject set, IEnumerable<GenericParameterHelper> other);

    /// <summary>
    /// Tests a set operation
    /// </summary>
    /// <param name="operation">The set operation that is tested</param>
    /// <param name="testObject">The object under test that will be passed to <paramref name="operation"/></param>
    /// <param name="other">The items parameter that will be passed to the <paramref name="operation"/></param>
    /// <param name="expected"></param>
    /// <param name="equalityComparer">comparer that is passed to the <paramref name="operation"/></param>
    protected void VerifySetOperation(
        SetOperation operation,
        TTestObject testObject,
        IEnumerable<GenericParameterHelper> other,
        GenericParameterHelper[] expected,
        bool isChangeExpected,
        IEqualityComparer<GenericParameterHelper>? equalityComparer,
        bool checkReferenceEquality = true)
    {
        if (!isChangeExpected && expected.Length != testObject.Count)
        {
            throw new ArgumentException();
        }

        var initialItems = testObject.ToList();
        var actual = operation(testObject, other);

        var itemComparerForVerification = checkReferenceEquality ? ReferenceEqualityComparer.Instance : equalityComparer;
        AssertCollectionsAreEqual(expected, actual, itemComparerForVerification);
        AssertCollectionsAreEqual(initialItems, testObject, itemComparerForVerification);

        if (isChangeExpected)
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

        AdditionalSetOperationVerification(testObject, other, expected, isChangeExpected, actual, equalityComparer, checkReferenceEquality);
    }

    protected virtual void AdditionalSetOperationVerification(
        TTestObject testObject,
        IEnumerable<GenericParameterHelper> other,
        GenericParameterHelper[] expected,
        bool isChangeExpected,
        IImmutableSet<GenericParameterHelper> actual,
        IEqualityComparer<GenericParameterHelper>? equalityComparer,
        bool checkReferenceEquality)
    { 
    }
}
