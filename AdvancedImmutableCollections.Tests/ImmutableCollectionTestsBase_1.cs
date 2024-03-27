using System.Collections;
using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.CollectionAdapters;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedImmutableCollections;

/// <summary>
/// Base class for tests of immutable collections.
/// </summary>
public abstract partial class ImmutableCollectionTestsBase<TFactory>
    where TFactory : IImmutableCollectionAdapterFactory, new()
{
    protected TFactory Factory { get; } = new();
    protected abstract Type GetTestObjectType<TItem>();

    protected abstract IEqualityTestStrategy EqualityTestStrategy { get; }

    [TestMethod]
    public void AddTest()
    {
        var expectedType = GetTestObjectType<GenericParameterHelper>();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        var expectedItems = Factory.CreateMutable<GenericParameterHelper>();
        var testObject = Factory.Create<GenericParameterHelper>();
        Add(item0);
        Add(item1);
        Add(item2);

        void Add(GenericParameterHelper item)
        {
            var itemsBefore = testObject.ToList();
            var actualResult = testObject.Add(item);
            Assert.AreNotSame(testObject, actualResult);
            Assert.IsTrue(actualResult.Contains(item));
            Assert.IsFalse(testObject.Contains(item));
            AssertCollectionsAreEqual(itemsBefore, testObject);
            expectedItems.Add(item);
            AssertCollectionsAreEqual(expectedItems, actualResult);

            Assert.IsInstanceOfType(actualResult, expectedType);
            testObject = Factory.Cast(actualResult);
        }
    }

    [TestMethod]
    public void RemoveTest()
    {
        var expectedType = GetTestObjectType<GenericParameterHelper>();
        var defaultValue = Factory.GetDefaultValue<GenericParameterHelper>();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var items = new List<GenericParameterHelper> { item0, item1, item2 };
        var expectedItems = Factory.CreateMutable(items.ToArray());
        var testObject = Factory.Create(item0, item1, item2);
        AssertCollectionsAreEqual(expectedItems, testObject);

        RemoveTest(new GenericParameterHelper(3), false);

        foreach (var itemToRemove in items)
        {
            RemoveTest(itemToRemove, true);
        }

        RemoveTest(item0, false);

        testObject = defaultValue;
        if (testObject is not null)
        {
            RemoveTest(item0, false);
        }

        void RemoveTest(GenericParameterHelper itemToRemove, bool expectRemoved)
        {
            var itemsBefore = testObject.ToList();
            var actualResult = testObject.Remove(itemToRemove);
            if (expectRemoved)
            {
                Assert.AreNotSame(testObject, actualResult);
                Assert.IsTrue(testObject.Contains(itemToRemove));
            }
            else if (defaultValue is not null)
            {
                // it's a value type so Assert.AreSame cannot be used
                Assert.AreEqual(testObject, actualResult);
                // this verification is redundant but if equals is incorrectly implemented, we are safe
                CollectionAssert.AreEqual(itemsBefore, actualResult.ToList());
            }
            else
            {
                Assert.AreSame(testObject, actualResult);
            }

            expectedItems.Remove(itemToRemove);
            Assert.IsFalse(actualResult.Contains(itemToRemove));
            AssertCollectionsAreEqual(itemsBefore, testObject);
            AssertCollectionsAreEqual(expectedItems, actualResult);

            Assert.IsInstanceOfType(actualResult, expectedType);
            testObject = Factory.Cast(actualResult);
        }
    }

    [TestMethod]
    public void ClearTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var testObject = Factory.Create(item0, item1);

        var itemsBefore = testObject.ToList();
        var actualResult = testObject.Clear();
        Assert.AreEqual(0, actualResult.Count);
        Assert.IsFalse(actualResult.Contains(item0));
        Assert.IsFalse(actualResult.Contains(item1));
        Assert.AreNotSame(testObject, actualResult);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = Factory.CreateMutable<GenericParameterHelper>();
        AssertCollectionsAreEqual(expected, actualResult);
    }

    [TestMethod]
    public void Clear_Empty_Test()
    {
        var testObject = Factory.Create<GenericParameterHelper>();
        var actual = testObject.Clear();
        Assert.AreEqual(0, actual.Count);
        AssertCollectionsAreEqual(testObject, actual);
    }

    [TestMethod]
    public void ContainsTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var testObject = Factory.Create(item0, item2);
        Assert.IsTrue(testObject.Contains(item0));
        Assert.IsFalse(testObject.Contains(item1));
        Assert.IsTrue(testObject.Contains(item2));

        testObject = Factory.Create<GenericParameterHelper>();
        Assert.IsFalse(testObject.Contains(item0));

        testObject = Factory.GetDefaultValue<GenericParameterHelper>();
        if (testObject is not null)
        {
            Assert.IsFalse(testObject.Contains(item0));
        }
    }

    [TestMethod]
    public void Contains_Empty_Test()
    {
        var item = new GenericParameterHelper(0);
        var testObject = Factory.Create<GenericParameterHelper>();
        Assert.IsFalse(testObject.Contains(item));
    }

    [TestMethod]
    public void AddRangeTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);

        var testObject = Factory.Create(item0, item1);
        var itemsBefore = testObject.ToList();
        var actualResult = testObject.AddRange([item2, item3, item4]);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = new GenericParameterHelper[] { item0, item1, item2, item3, item4 };
        AssertCollectionsAreEqual(expected, actualResult);
        Assert.IsTrue(actualResult.Contains(item2));
        Assert.IsTrue(actualResult.Contains(item3));
        Assert.IsTrue(actualResult.Contains(item4));
    }

    [TestMethod]
    public void CountTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        CountTest([]);
        CountTest([item0]);
        CountTest([item0, item1, item2]);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } defaultValue)
        {
            var actual = defaultValue.Count;
            Assert.AreEqual(0, actual);
        }

        void CountTest(GenericParameterHelper[] items)
        {
            var testObject = Factory.Create(items);
            Assert.AreEqual(items.Length, testObject.Count);
        }
    }

    [TestMethod]
    public void EqualsTest()
    {
        EqualityTestStrategy.EqualsTest(Factory);
    }

    [TestMethod]
    public void GetHashCodeTest()
    {
        EqualityTestStrategy.GetHashCodeTest(Factory);

        if (Factory.GetDefaultValue<GenericParameterHelper>() is { } defaultValue)
        {
            var hashCodeDefault = defaultValue.GetHashCode();
            var hashCodeEmpty = Factory.Create<GenericParameterHelper>().GetHashCode();
            Assert.AreEqual(hashCodeDefault, hashCodeEmpty);
        }
    }

    protected virtual void AssertCollectionsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer = null)
        => CollectionAssert.That.AreEqual(expected, actual, itemComparer);
}
