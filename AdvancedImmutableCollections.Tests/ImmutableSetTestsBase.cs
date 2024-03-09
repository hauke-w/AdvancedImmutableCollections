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
    protected abstract IImmutableSet<GenericParameterHelper> Except(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> Union(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> Intersect(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract IImmutableSet<GenericParameterHelper> SymmetricExcept(TTestObject collection, IEnumerable<GenericParameterHelper> other);

    protected abstract bool SetEquals(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract bool IsSupersetOf(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract bool IsSubsetOf(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract bool IsProperSupersetOf(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract bool IsProperSubsetOf(TTestObject collection, IEnumerable<GenericParameterHelper> other);
    protected abstract bool Overlaps(TTestObject collection, IEnumerable<GenericParameterHelper> other);

    protected abstract bool TryGetValue(TTestObject collection, GenericParameterHelper equalValue, out GenericParameterHelper actualValue);

    [TestMethod]
    public void TryGetTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        TryGetTest([], item0, false, item0);
        TryGetTest([item0], item0, true, item0);
        TryGetTest([item0], item0b, true, item0);
        TryGetTest([item0], item1, false, item1);
        TryGetTest([item0, item1], item0, true, item0);
        TryGetTest([item0, item1], item0b, true, item0);
        TryGetTest([item0, item1], item1, true, item1);
        TryGetTest([item0, item1], item2, false, item2);

        if (DefaultValue is not null)
        {
            TryGetTestCore(DefaultValue, item0, false, item0);
        }

        void TryGetTest(GenericParameterHelper[] items, GenericParameterHelper equalValue, bool expected, GenericParameterHelper expectedActualValue)
        {
            var testObject = CreateInstance(items);
            TryGetTestCore(testObject, equalValue, expected, expectedActualValue);
        }

        void TryGetTestCore(TTestObject testObject, GenericParameterHelper equalValue, bool expected, GenericParameterHelper expectedActualValue)
        {
            var actual = TryGetValue(testObject, equalValue, out var actualValue);
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(expectedActualValue, actualValue);
        }
    }

    [TestMethod]
    public void SetEqualsTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        SetEqualsTest([], [], true);
        SetEqualsTest([item0], [item0], true);
        SetEqualsTest([item0], [item0b], true);
        SetEqualsTest([item0, item1, item2], [item0, item1, item2], true);
        SetEqualsTest([item0], [item0, item1], false);
        SetEqualsTest([item0, item1], [item0], false);
        SetEqualsTest([item0, item1], [item0, item2], false);
        SetEqualsTest([], (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        SetEqualsTest([], new HashSet<GenericParameterHelper>(), true);
        SetEqualsTest([item0, item1, item2], new List<GenericParameterHelper>() { item0, item1, item0, item2, item1 }, true);

        if (DefaultValue is not null)
        {
            VerifySetEquals(DefaultValue, [], true);
            VerifySetEquals(DefaultValue, DefaultValue, true);
            SetEqualsTest([], DefaultValue, true);
            VerifySetEquals(DefaultValue, [item0], false);
            SetEqualsTest([item0], DefaultValue, false);
            VerifySetEquals(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void SetEqualsTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifySetEquals(testObject, other, expected);
        }

    }
    protected void VerifySetEquals(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = SetEquals(testObject, other);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void IsSupersetOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        IsSupersetOfTest([], [], true);
        IsSupersetOfTest([item0], [], true);
        IsSupersetOfTest([], [item0], false);
        IsSupersetOfTest([item0], [item0], true);
        IsSupersetOfTest([item0], [item0b], true);
        IsSupersetOfTest([item0], [item0, item1], false);
        IsSupersetOfTest([item0, item1, item2], [item0, item1b], true);
        IsSupersetOfTest([item0, item1, item2], [item0b, item1b, item2], true);

        if (DefaultValue is not null)
        {
            VerifyIsSupersetOf(DefaultValue, [], true);
            VerifyIsSupersetOf(DefaultValue, DefaultValue, true);
            IsSupersetOfTest([], DefaultValue, true);
            VerifyIsSupersetOf(DefaultValue, [item0], false);
            IsSupersetOfTest([item0], DefaultValue, true);
            VerifyIsSupersetOf(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void IsSupersetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifyIsSupersetOf(testObject, other, expected);
        }
    }

    protected void VerifyIsSupersetOf(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = IsSupersetOf(testObject, other);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void IsSubsetOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        IsSubsetOfTest([], [], true);
        IsSubsetOfTest([], [item0], true);
        IsSubsetOfTest([item0], [], false);
        IsSubsetOfTest([item0], [item0], true);
        IsSubsetOfTest([item0], [item0b], true);
        IsSubsetOfTest([item0, item1], [item0, item1b, item2], true);

        if (DefaultValue is not null)
        {
            VerifyIsSubsetOf(DefaultValue, [], true);
            VerifyIsSubsetOf(DefaultValue, DefaultValue, true);
            IsSubsetOfTest([], DefaultValue, true);
            VerifyIsSubsetOf(DefaultValue, [item0], true);
            IsSubsetOfTest([item0], DefaultValue, false);
            VerifyIsSubsetOf(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void IsSubsetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifyIsSubsetOf(testObject, other, expected);
        }
    }

    protected void VerifyIsSubsetOf(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = IsSubsetOf(testObject, other);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void IsProperSupersetOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        IsProperSupersetOfTest([], [], false);
        IsProperSupersetOfTest([], [item0], false);
        IsProperSupersetOfTest([item0], [], true);
        IsProperSupersetOfTest([item0], [item0], false);
        IsProperSupersetOfTest([item0, item1, item2], [item0, item1], true);
        IsProperSupersetOfTest([item0, item1, item2], [item0, item1b], true);

        if (DefaultValue is not null)
        {
            VerifyIsProperSupersetOf(DefaultValue, [], false);
            VerifyIsProperSupersetOf(DefaultValue, DefaultValue, false);
            IsProperSupersetOfTest([], DefaultValue, false);
            VerifyIsProperSupersetOf(DefaultValue, [item0], false);
            IsProperSupersetOfTest([item0], DefaultValue, true);
            VerifyIsProperSupersetOf(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void IsProperSupersetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifyIsProperSupersetOf(testObject, other, expected);
        }
    }

    protected void VerifyIsProperSupersetOf(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = IsProperSupersetOf(testObject, other);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void IsProperSubsetOfTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        IsProperSubsetOfTest([], [], false);
        IsProperSubsetOfTest([], [item0], true);
        IsProperSubsetOfTest([item0], [], false);
        IsProperSubsetOfTest([item0], [item0], false);
        IsProperSubsetOfTest([item0], [item0b], false);
        IsProperSubsetOfTest([item0, item1], [item0, item1b, item2], true);

        if (DefaultValue is not null)
        {
            VerifyIsProperSubsetOf(DefaultValue, [], false);
            VerifyIsProperSubsetOf(DefaultValue, DefaultValue, false);
            IsProperSubsetOfTest([], DefaultValue, false);
            VerifyIsProperSubsetOf(DefaultValue, [item0], true);
            IsProperSubsetOfTest([item0], DefaultValue, false);
            VerifyIsProperSubsetOf(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void IsProperSubsetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifyIsProperSubsetOf(testObject, other, expected);
        }
    }
    protected void VerifyIsProperSubsetOf(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = IsProperSubsetOf(testObject, other);
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void OverlapsTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        OverlapsTest([], [], false);
        OverlapsTest([], [item0], false);
        OverlapsTest([item0], [], false);
        OverlapsTest([item0], [item0], true);
        OverlapsTest([item0], [item0b], true);

        OverlapsTest([item0, item1], [item1b, item2], true);
        OverlapsTest([item0], [item0, item1], true);
        OverlapsTest([item0, item1], [item1], true);
        OverlapsTest([item0], [item1, item2], false);

        if (DefaultValue is not null)
        {
            VerifyOverlaps(DefaultValue, [], false);
            VerifyOverlaps(DefaultValue, DefaultValue, false);
            OverlapsTest([], DefaultValue, false);
            VerifyOverlaps(DefaultValue, [item0], false);
            OverlapsTest([item0], DefaultValue, false);
            VerifyOverlaps(DefaultValue, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void OverlapsTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObject = CreateInstance(items);
            VerifyOverlaps(testObject, other, expected);
        }
    }
    protected void VerifyOverlaps(TTestObject testObject, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        var actual = Overlaps(testObject, other);
        Assert.AreEqual(expected, actual);
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
    /// <param name="equalityComparerForVerification">comparer that is used for verifying elements in the result</param>
    protected void VerifySetOperation(
        SetOperation operation,
        TTestObject testObject,
        IEnumerable<GenericParameterHelper> other,
        GenericParameterHelper[] expected,
        bool isChangeExpected,
        IEqualityComparer<GenericParameterHelper>? equalityComparerForVerification)
    {
        if (!isChangeExpected && expected.Length != testObject.Count)
        {
            throw new ArgumentException();
        }

        var initialItems = testObject.ToList();
        var actual = operation(testObject, other);

        AssertCollectionsAreEqual(expected, actual, equalityComparerForVerification);
        AssertCollectionsAreEqual(initialItems, testObject, equalityComparerForVerification);

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

        AdditionalSetOperationVerification(testObject, other, expected, isChangeExpected, actual, equalityComparerForVerification);
    }

    protected virtual void AdditionalSetOperationVerification(
        TTestObject testObject,
        IEnumerable<GenericParameterHelper> other,
        GenericParameterHelper[] expected,
        bool isChangeExpected,
        IImmutableSet<GenericParameterHelper> actual,
        IEqualityComparer<GenericParameterHelper>? equalityComparer)
    {
    }

    public abstract void GetEnumeratorTest();
    public abstract void IEnumerable_GetEnumeratorTest();
    public abstract void ExceptTest();
    public abstract void UnionTest();
    public abstract void IntersectTest();
    public abstract void SymmetricExceptTest();
}