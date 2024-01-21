using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1Tests : ImmutableSetTestsBase<ImmutableHashSetValue<GenericParameterHelper>>
{
#if NET6_0_OR_GREATER
    protected override ISetEqualityWithEqualityComparerTestStrategy EqualityTestStrategy
#else
    protected override ISetEqualityWithEqualityComparerTestStrategy SetEqualityTestStrategy
#endif
        => SetValueEqualityTestStrategy.Default;

    internal protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance() => new ImmutableHashSetValue<GenericParameterHelper>();

    internal protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance(params GenericParameterHelper[] initialItems)
        => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());
    protected override IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems) => new ImmutableHashSetValue<T>(initialItems.ToImmutableArray());
    protected override ImmutableHashSetValue<GenericParameterHelper> CreateInstance(HashSet<GenericParameterHelper> source)
        => new ImmutableHashSetValue<GenericParameterHelper>(source);

    protected override IReadOnlyCollection<T>? GetDefaultValue<T>() => default(ImmutableHashSetValue<T>);

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> AddRange(ImmutableHashSetValue<GenericParameterHelper> collection, params GenericParameterHelper[] newItems) => collection.Union(newItems);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.Clear();

    protected override IImmutableSet<GenericParameterHelper> Except(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Except(other);
    protected override IImmutableSet<GenericParameterHelper> Union(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Union(other);
    protected override IImmutableSet<GenericParameterHelper> Intersect(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.Intersect(other);
    protected override IImmutableSet<GenericParameterHelper> SymmetricExcept(ImmutableHashSetValue<GenericParameterHelper> collection, IEnumerable<GenericParameterHelper> other)
        => collection.SymmetricExcept(other);

    protected override bool Contains(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Contains(item);

    protected override IEnumerator<GenericParameterHelper> GetEnumerator(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.GetEnumerator();

    public override bool VerifyIntersectWithReferenceEquality => false; // do not check reference equality because the underlying ImmutableHashSet takes elements from the other collection

    [TestMethod]
    public void Ctor_IEnumerable_Test()
    {
        Ctor_ImmutableHashSet_Test(ImmutableHashSet<GenericParameterHelper>.Empty);
        Ctor_ImmutableHashSet_Test(ImmutableHashSet.Create(1, 2, 3));

        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        Ctor_ImmutableHashSet_Test(new HashSet<GenericParameterHelper>([item0, item1, item0b, item2, item1b], ReferenceEqualityComparer.Instance));
        Ctor_ImmutableHashSet_Test(new List<GenericParameterHelper> { item0, item1, item0b, item2, item1b });
        Ctor_ImmutableHashSet_Test(new List<GenericParameterHelper> { item2, item1, item0 });
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableHashSetValue<GenericParameterHelper>((IEnumerable<GenericParameterHelper>)null!));

        void Ctor_ImmutableHashSet_Test<T>(IEnumerable<T> items)
        {
            var actual = new ImmutableHashSetValue<T>(items);

            List<T> uniqueItems;
            if (items is HashSet<T> set)
            {
                Assert.AreSame(set.Comparer, actual.Value.KeyComparer);
                uniqueItems = set.ToList();
            }
            else
            {
                uniqueItems = items.Distinct().ToList();
            }
            CollectionAssert.AreEquivalent(uniqueItems, actual.ToList());
            Assert.IsFalse(actual.IsDefault);
        }
    }

    [TestMethod]
    public void Ctor_ImmutableHashSet_Test()
    {
        Ctor_ImmutableHashSet_Test(ImmutableHashSet<GenericParameterHelper>.Empty);
        Ctor_ImmutableHashSet_Test(ImmutableHashSet.Create(1, 2, 3));
        Assert.ThrowsException<ArgumentNullException>(() => new ImmutableHashSetValue<GenericParameterHelper>((ImmutableHashSet<GenericParameterHelper>)null!));

        void Ctor_ImmutableHashSet_Test<T>(ImmutableHashSet<T> set)
        {
            var actual = new ImmutableHashSetValue<T>(set);
            Assert.AreSame(set, actual.Value);
            Assert.IsFalse(actual.IsDefault);
        }
    }

    [TestMethod]
    public void Ctor_IEqualityComparer_Test()
    {
        Ctor_EqualityComparer_Test(null, EqualityComparer<GenericParameterHelper>.Default);
        Ctor_EqualityComparer_Test(EqualityComparer<GenericParameterHelper>.Default, EqualityComparer<GenericParameterHelper>.Default);
        Ctor_EqualityComparer_Test(ReferenceEqualityComparer.Instance, ReferenceEqualityComparer.Instance);

        void Ctor_EqualityComparer_Test<T>(IEqualityComparer<T>? equalityComparer, IEqualityComparer<T> expectedComparer)
        {
            var actual = new ImmutableHashSetValue<T>(equalityComparer);
            Assert.AreSame(expectedComparer, actual.Value.KeyComparer);
            Assert.IsFalse(actual.IsDefault);
        }
    }

    [TestMethod]
    public void Ctor_IEqualityComparer_IEnumerable_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        Ctor_IEqualityComparer_IEnumerable_Test(null, [], EqualityComparer<GenericParameterHelper>.Default, []);
        Ctor_IEqualityComparer_IEnumerable_Test(EqualityComparer<GenericParameterHelper>.Default, [item0], EqualityComparer<GenericParameterHelper>.Default, [item0]);
        Ctor_IEqualityComparer_IEnumerable_Test(null, [item0, item1, item0b], EqualityComparer<GenericParameterHelper>.Default, [item0, item1]);
        Ctor_IEqualityComparer_IEnumerable_Test(ReferenceEqualityComparer.Instance, [item0, item1, item0b], ReferenceEqualityComparer.Instance, [item0, item1, item0b]);
        Ctor_IEqualityComparer_IEnumerable_Test(ReferenceEqualityComparer.Instance, [item0, item1, item0b, item0, item1, item1b, item2], ReferenceEqualityComparer.Instance, [item0, item1, item0b, item1b, item2]);

        Ctor_IEqualityComparer_IEnumerable_Test(null, new HashSet<GenericParameterHelper>(new[] { item0, item0b }, ReferenceEqualityComparer.Instance), EqualityComparer<GenericParameterHelper>.Default, [item0]);

        void Ctor_IEqualityComparer_IEnumerable_Test<T>(IEqualityComparer<T>? equalityComparer, IEnumerable<T> items, IEqualityComparer<T> expectedComparer, T[] expectedItems)
        {
            var actual = new ImmutableHashSetValue<T>(equalityComparer, items);
            Assert.AreSame(expectedComparer, actual.Value.KeyComparer);
            Assert.IsFalse(actual.IsDefault);
            CollectionAssert.AreEquivalent(expectedItems, actual.ToList());
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.op_Implicit(ImmutableHashSetValue{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetTest()
    {
        var source = ImmutableHashSet.Create(1, 2, 3);
        var value = new ImmutableHashSetValue<int>(source);
        ImmutableHashSet<int> actual = value; // do the conversion ImmutableHashSetValue<int> -> ImmutableHashSet<int>
        Assert.IsTrue(source.SetEquals(actual));
        Assert.AreSame(source, actual);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.op_Implicit(ImmutableHashSet{T})"/>
    /// </summary>
    [TestMethod]
    public void OpToImmutableHashSetValueTest()
    {
        var value = ImmutableHashSet.Create(1, 2, 3);
        ImmutableHashSetValue<int> actual = value; // do the conversion ImmutableHashSet<int> -> ImmutableHashSetValue<int>
        var expected = new ImmutableHashSetValue<int>(value);
        Assert.AreEqual(expected, actual);
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.operator==)"/>
    /// </summary>
    [TestMethod]
    public void OpEqualsTest()
    {
        var testCases = GetOpEqualsTestCases();
        for (int i = 0; i < testCases.Length; i++)
        {
            var (left, right, expected) = testCases[i];
            var actual = left == right;
            Assert.AreEqual(expected, actual);
        }
    }

    /// <summary>
    /// Verifies <see cref="ImmutableHashSetValue{T}.operator!=)"/>
    /// </summary>
    [TestMethod]
    public void OpNotEqualsTest()
    {
        var testCases = GetOpEqualsTestCases();
        for (int i = 0; i < testCases.Length; i++)
        {
            var (left, right, expectedEqual) = testCases[i];
            var actual = left != right;
            Assert.AreEqual(!expectedEqual, actual);
        }
    }

    private static (ImmutableHashSetValue<GenericParameterHelper> Left, ImmutableHashSetValue<GenericParameterHelper> Right, bool Expected)[] GetOpEqualsTestCases()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        return
            [
                (default, default, true),

                // default is never equal to non-default
                (default, new(ReferenceEqualityComparer.Instance), false),
                (new(ReferenceEqualityComparer.Instance), default, false),
                (default, new(EqualityComparer<GenericParameterHelper>.Default), false),
                (new(EqualityComparer<GenericParameterHelper>.Default), default, false),

                // empty instances are equal if they have equal comparers
                (new(EqualityComparer<GenericParameterHelper>.Default), new(ReferenceEqualityComparer.Instance), false),
                (new(ReferenceEqualityComparer.Instance), new(ReferenceEqualityComparer.Instance), true),
                (new(EqualityComparer<GenericParameterHelper>.Default), new((IEqualityComparer<GenericParameterHelper>?)null), true),

                (new(EqualityComparer<GenericParameterHelper>.Default, [item0]), new(null, [item0]), true),
                (new(ReferenceEqualityComparer.Instance, [item0]), new(null, [item0]), false),
                (new(null, [item0]), new(ReferenceEqualityComparer.Instance, [item0]), false),
                (new(ReferenceEqualityComparer.Instance, [item0]), new(ReferenceEqualityComparer.Instance, [item0b]), false),
                (new(ReferenceEqualityComparer.Instance, [item0]), new(ReferenceEqualityComparer.Instance, [item0]), true),
                (new(EqualityComparer<GenericParameterHelper>.Default, [item0, item1]), new(null, [item0, item1]), true),
                (new(EqualityComparer<GenericParameterHelper>.Default, [item0, item1]), new(null, [item0, item2]), false),
                (new(EqualityComparer<GenericParameterHelper>.Default, [item0, item1, item2]), new(null, [item0, item2]), false),
                (new(null, [item0]), new(null, [item0, item1]), false),
                (new(ReferenceEqualityComparer.Instance, [item0, item1, item1b]), new(null, [item0, item1]), false),
                (new(null, [item0, item1, item1b]), new(ReferenceEqualityComparer.Instance, [item0, item1]), false),
                (new(ReferenceEqualityComparer.Instance, [item0, item1, item1b, item0b, item2]), new(ReferenceEqualityComparer.Instance, [item2, item1, item1b, item0b, item0]), true),
            ];
    }

    [TestMethod]
    public void IsDefaultTest()
    {
        Assert.IsTrue(default(ImmutableHashSetValue<GenericParameterHelper>).IsDefault);
        Assert.IsTrue(new ImmutableHashSetValue<GenericParameterHelper>().IsDefault);
        Assert.IsFalse(new ImmutableHashSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefault);
        GenericParameterHelper[] items = [new()];
        Assert.IsFalse(new ImmutableHashSetValue<GenericParameterHelper>(items).IsDefault);
    }

    [TestMethod]
    public void IsDefaultOrEmptyTest()
    {
        Assert.IsTrue(default(ImmutableHashSetValue<GenericParameterHelper>).IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableHashSetValue<GenericParameterHelper>().IsDefaultOrEmpty);
        Assert.IsTrue(new ImmutableHashSetValue<GenericParameterHelper>(ImmutableArray<GenericParameterHelper>.Empty).IsDefaultOrEmpty);
        var items = new GenericParameterHelper[] { new GenericParameterHelper() };
        Assert.IsFalse(new ImmutableHashSetValue<GenericParameterHelper>(items).IsDefaultOrEmpty);
    }

    [TestMethod]
    public void WithComparerTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item2b = new GenericParameterHelper(2);

        WithComparerTest<GenericParameterHelper>([], null, ReferenceEqualityComparer.Instance, ReferenceEqualityComparer.Instance, []);
        WithComparerTest<GenericParameterHelper>([item0, item1, item2], null, ReferenceEqualityComparer.Instance, ReferenceEqualityComparer.Instance, [item0, item1, item2]);
        WithComparerTest<GenericParameterHelper>([item0, item1, item0b], ReferenceEqualityComparer.Instance, null, EqualityComparer<GenericParameterHelper>.Default, [item0, item1]);
        WithComparerTest<GenericParameterHelper>([item0, item1, item0b, item2, item2b], ReferenceEqualityComparer.Instance, EqualityComparer<GenericParameterHelper>.Default, EqualityComparer<GenericParameterHelper>.Default, [item0, item1, item2]);

        var set = ImmutableHashSet.Create(item0, item1, item2).WithValueSemantics();
        WithComparerTestCore(set, null, set);
        set = ImmutableHashSet.Create<GenericParameterHelper>(ReferenceEqualityComparer.Instance, item0, item1, item0b).WithValueSemantics();
        WithComparerTestCore(set, ReferenceEqualityComparer.Instance, set);

        WithComparerTestCore<GenericParameterHelper>(default, null, default);
        WithComparerTestCore(default, ReferenceEqualityComparer.Instance, new ImmutableHashSetValue<GenericParameterHelper>(ReferenceEqualityComparer.Instance));

        void WithComparerTest<T>(GenericParameterHelper[] testObjectItems, IEqualityComparer<GenericParameterHelper>? initialEqualityComparer, IEqualityComparer<GenericParameterHelper>? newEqualityComparer, IEqualityComparer<GenericParameterHelper> expectedEqualityComparer, GenericParameterHelper[] expectedItems)
        {
            var testObject = testObjectItems.ToImmutableHashSet(initialEqualityComparer).WithValueSemantics();
            var expected = expectedItems.ToImmutableHashSet(expectedEqualityComparer).WithValueSemantics();
            WithComparerTestCore(testObject, newEqualityComparer, expected);
        }

        void WithComparerTestCore<T>(ImmutableHashSetValue<T> testObject, IEqualityComparer<T>? comparer, ImmutableHashSetValue<T> expected)
        {
            var actual = testObject.WithComparer(comparer);
            Assert.IsTrue(expected == actual);
        }
    }

    protected override void AdditionalSetOperationVerification(
        ImmutableHashSetValue<GenericParameterHelper> testObject,
        IEnumerable<GenericParameterHelper> other,
        GenericParameterHelper[] expected,
        bool isChangeExpected,
        IImmutableSet<GenericParameterHelper> actual,
        IEqualityComparer<GenericParameterHelper>? equalityComparer,
        bool checkReferenceEquality)
    {
        if (actual is ImmutableHashSetValue<GenericParameterHelper> actualValue)
        {
            Assert.AreEqual(testObject.Value.KeyComparer, actualValue.Value.KeyComparer, "The actual result does not have the original comparer");

            if (isChangeExpected)
            {
                Assert.IsFalse(actualValue.IsDefault);
            }
            else
            {
                Assert.AreEqual(testObject.IsDefault, actualValue.IsDefault);
            }
        }
        else
        {
            Assert.Fail($"Unexpected type {actual.GetType()}");
        }
    }
}
