using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract partial class ImmutableSetTestsBase<TFactory> : ImmutableCollectionTestsBase<TFactory>
    where TFactory : IImmutableSetAdapterFactory, new()
{
    private static ICollection<T> EmptyICollectionMock<T>()
    {
        var mock = new Mock<ICollection<T>>();
        mock.Setup(x => x.Count).Returns(0);
        return mock.Object;
    }

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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            TryGetTestCore(@default, item0, false, item0);
        }

        void TryGetTest(GenericParameterHelper[] items, GenericParameterHelper equalValue, bool expected, GenericParameterHelper expectedActualValue)
        {
            var testObjectAdapter = Factory.Create(items);
            TryGetTestCore(testObjectAdapter, equalValue, expected, expectedActualValue);
        }

        void TryGetTestCore(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, GenericParameterHelper equalValue, bool expected, GenericParameterHelper expectedActualValue)
        {
            var actual = testObjectAdapter.TryGetValue(equalValue, out var actualValue);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifySetEquals(@default, [], true);
            VerifySetEquals(@default, @default, true);
            SetEqualsTest([], @default, true);
            VerifySetEquals(@default, [item0], false);
            SetEqualsTest([item0], @default, false);
            VerifySetEquals(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void SetEqualsTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifySetEquals(testObjectAdapter, other, expected);
        }

    }

    protected void VerifySetEquals<T>(IImmutableSetAdapter<T> testObjectAdapter, IEnumerable<T> other, bool expected)
    {
        if (other is ICollectionAdapter<T> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.SetEquals(other);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifyIsSupersetOf(@default, [], true);
            VerifyIsSupersetOf(@default, EmptyICollectionMock<GenericParameterHelper>(), true);
            VerifyIsSupersetOf(@default, @default, true);
            IsSupersetOfTest([], @default, true);
            VerifyIsSupersetOf(@default, [item0], false);
            IsSupersetOfTest([item0], @default, true);
            VerifyIsSupersetOf(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void IsSupersetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifyIsSupersetOf(testObjectAdapter, other, expected);
        }
    }

    protected void VerifyIsSupersetOf(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        if (other is ICollectionAdapter<GenericParameterHelper> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.IsSupersetOf(other);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifyIsSubsetOf(@default, [], true);
            VerifyIsSubsetOf(@default, EmptyICollectionMock<GenericParameterHelper>(), true);
            VerifyIsSubsetOf(@default, @default, true);
            IsSubsetOfTest([], @default, true);
            VerifyIsSubsetOf(@default, [item0], true);
            IsSubsetOfTest([item0], @default, false);
            VerifyIsSubsetOf(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), true);
        }

        void IsSubsetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifyIsSubsetOf(testObjectAdapter, other, expected);
        }
    }

    protected void VerifyIsSubsetOf(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        if (other is ICollectionAdapter<GenericParameterHelper> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.IsSubsetOf(other);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifyIsProperSupersetOf(@default, [], false);
            VerifyIsProperSupersetOf(@default, EmptyICollectionMock<GenericParameterHelper>(), false);
            VerifyIsProperSupersetOf(@default, @default, false);
            IsProperSupersetOfTest([], @default, false);
            VerifyIsProperSupersetOf(@default, [item0], false);
            IsProperSupersetOfTest([item0], @default, true);
            VerifyIsProperSupersetOf(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void IsProperSupersetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifyIsProperSupersetOf(testObjectAdapter, other, expected);
        }
    }

    protected void VerifyIsProperSupersetOf(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        if (other is ICollectionAdapter<GenericParameterHelper> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.IsProperSupersetOf(other);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifyIsProperSubsetOf(@default, [], false);
            VerifyIsProperSubsetOf(@default, EmptyICollectionMock<GenericParameterHelper>(), false);
            VerifyIsProperSubsetOf(@default, @default, false);
            IsProperSubsetOfTest([], @default, false);
            VerifyIsProperSubsetOf(@default, [item0], true);
            IsProperSubsetOfTest([item0], @default, false);
            VerifyIsProperSubsetOf(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void IsProperSubsetOfTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifyIsProperSubsetOf(testObjectAdapter, other, expected);
        }
    }
    protected void VerifyIsProperSubsetOf(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        if (other is ICollectionAdapter<GenericParameterHelper> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.IsProperSubsetOf(other);
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

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } @default)
        {
            VerifyOverlaps(@default, [], false);
            VerifyOverlaps(@default, EmptyICollectionMock<GenericParameterHelper>(), false);
            VerifyOverlaps(@default, @default, false);
            OverlapsTest([], @default, false);
            VerifyOverlaps(@default, [item0], false);
            OverlapsTest([item0], @default, false);
            VerifyOverlaps(@default, (new GenericParameterHelper[] { }).WrapAsEnumerable(), false);
        }

        void OverlapsTest(GenericParameterHelper[] items, IEnumerable<GenericParameterHelper> other, bool expected)
        {
            var testObjectAdapter = Factory.Create(items);
            VerifyOverlaps(testObjectAdapter, other, expected);
        }
    }
    protected void VerifyOverlaps(IImmutableSetAdapter<GenericParameterHelper> testObjectAdapter, IEnumerable<GenericParameterHelper> other, bool expected)
    {
        if (other is ICollectionAdapter<GenericParameterHelper> otherAdapter)
        {
            other = otherAdapter.Collection;
        }
        var actual = testObjectAdapter.Overlaps(other);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// A function that will produce a manipulated set from the <paramref name="set"/> and <paramref name="other"/> parameter.
    /// </summary>
    /// <param name="set">The set on which the operation is executed</param>
    /// <param name="other">parameter of the operation that will be used to compute the result</param>
    /// <returns></returns>
    protected delegate IImmutableSet<T> SetOperation<T>(IImmutableSetAdapter<T> set, IEnumerable<T> other);

    protected void VerifySetOperation<T>(
        SetOperation<T> operation,
        IImmutableSetAdapter<T> testObjectAdapter,
        IEnumerable<T> other,
        T[] expected,
        bool isChangeExpected,
        IEqualityComparer<T>? equalityComparerForVerification)
    {
        if (!isChangeExpected && expected.Length != testObjectAdapter.Count)
        {
            throw new ArgumentException();
        }

        var initialItems = testObjectAdapter.ToList();
        var actual = operation(testObjectAdapter, other);

        AssertCollectionsAreEqual(expected, actual, equalityComparerForVerification);
        AssertCollectionsAreEqual(initialItems, testObjectAdapter, equalityComparerForVerification);

        if (isChangeExpected)
        {
            Assert.AreNotSame(testObjectAdapter.Collection, actual);
        }
        else if (Factory.GetDefaultValue<GenericParameterHelper>() is not null)
        {
            // it's a value type so Assert.AreSame cannot be used
            Assert.AreEqual(testObjectAdapter.Collection, actual);
        }
        else
        {
            Assert.AreSame(testObjectAdapter.Collection, actual);
        }

        AdditionalSetOperationVerification(testObjectAdapter, other, expected, isChangeExpected, actual, equalityComparerForVerification);
    }

    protected virtual void AdditionalSetOperationVerification<T>(
        IImmutableSetAdapter<T> testObjectAdapter,
        IEnumerable<T> other,
        T[] expected,
        bool isChangeExpected,
        IImmutableSet<T> actual,
        IEqualityComparer<T>? equalityComparer)
    {
    }

    public abstract void GetEnumeratorTest();
    public abstract void IEnumerable_GetEnumeratorTest();
    public abstract void ExceptTest();
    public abstract void UnionTest();
    public abstract void IntersectTest();
    public abstract void SymmetricExceptTest();
}