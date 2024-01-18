using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

public abstract class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    protected abstract TTestObject GetTestObject();
    protected abstract TTestObject GetTestObject(params GenericParameterHelper[] initialItems);
    protected abstract TMutable GetMutableCollection(params GenericParameterHelper[] initialItems);

    protected abstract IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item);
    protected abstract IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item);
    protected abstract IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection);
    protected abstract IReadOnlyCollection<GenericParameterHelper> AddRange(TTestObject collection, params GenericParameterHelper[] newItems);
    protected abstract bool Contains(TTestObject collection, GenericParameterHelper item);

    protected bool Contains(IReadOnlyCollection<GenericParameterHelper> collection, GenericParameterHelper item)
    {
        return collection switch
        {
            TTestObject c => Contains(c, item),
            IImmutableSet<GenericParameterHelper> c => c.Contains(item),
            ImmutableArray<GenericParameterHelper> c => c.Contains(item),
            ICollection<GenericParameterHelper> c => c.Contains(item),
            _ => collection.Contains(item),
        };
    }

    [TestMethod]
    public void AddTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        var expectedItems = GetMutableCollection();
        var testObject = GetTestObject();
        Add(item0);
        Add(item1);
        Add(item2);

        void Add(GenericParameterHelper item)
        {
            var itemsBefore = testObject.ToList();
            var actualResult = this.Add(testObject, item);
            Assert.AreNotSame(testObject, actualResult);
            Assert.IsTrue(actualResult.Contains(item));
            Assert.IsFalse(testObject.Contains(item));
            AssertCollectionsAreEqual(itemsBefore, testObject);
            expectedItems.Add(item);
            AssertCollectionsAreEqual(expectedItems, actualResult);

            if (actualResult is not TTestObject newTestObject)
            {
                Assert.Fail("actual is not of expected type");
                return;
            }
            testObject = newTestObject;
        }
    }

    [TestMethod]
    public void RemoveTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var items = new List<GenericParameterHelper> { item0, item1, item2 };
        var expectedItems = GetMutableCollection(item0, item1, item2);
        var testObject = GetTestObject(item0, item1, item2);
        AssertCollectionsAreEqual(expectedItems, testObject);

        foreach (var itemToRemove in items)
        {
            var itemsBefore = testObject.ToList();
            var actualResult = Remove(testObject, itemToRemove);
            Assert.AreNotSame(testObject, actualResult);

            expectedItems.Remove(itemToRemove);
            Assert.IsFalse(actualResult.Contains(itemToRemove));
            Assert.IsTrue(Contains(testObject, itemToRemove));
            AssertCollectionsAreEqual(itemsBefore, testObject);
            AssertCollectionsAreEqual(expectedItems, actualResult);

            if (actualResult is not TTestObject newTestObject)
            {
                Assert.Fail("actual is not of expected type");
                return;
            }
            testObject = newTestObject;
        }
    }

    [TestMethod]
    public void ClearTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var testObject = GetTestObject(item0, item1);

        var itemsBefore = testObject.ToList();
        var actualResult = Clear(testObject);
        Assert.AreEqual(0, actualResult.Count);
        Assert.IsFalse(actualResult.Contains(item0));
        Assert.IsFalse(actualResult.Contains(item1));
        Assert.AreNotSame(testObject, actualResult);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = GetMutableCollection();
        AssertCollectionsAreEqual(expected, actualResult);

        // clear empty collection
        testObject = GetTestObject();
    }

    [TestMethod]
    public void Clear_Empty_Test()
    {
        var testObject = GetTestObject();
        var actual = Clear(testObject);
        Assert.AreEqual(0, actual.Count);
        Assert.AreEqual(testObject, actual);
    }

    [TestMethod]
    public void ContainsTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var testObject = GetTestObject(item0, item2);
        Assert.IsTrue(Contains(testObject, item0));
        Assert.IsFalse(Contains(testObject, item1));
        Assert.IsTrue(Contains(testObject, item2));

        testObject = GetTestObject();
        Assert.IsFalse(testObject.Contains(item0));
    }

    [TestMethod]
    public void Contains_Empty_Test()
    {
        var item = new GenericParameterHelper(0);
        var testObject = GetTestObject();
        Assert.IsFalse(Contains(testObject, item));
    }

    [TestMethod]
    public void AddRangeTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);

        var testObject = GetTestObject(item0, item1);

        var itemsBefore = testObject.ToList();
        var actualResult = AddRange(testObject, item2, item3, item4);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = new GenericParameterHelper[] { item0, item1, item2, item3, item4 };
        AssertCollectionsAreEqual(expected, actualResult);
        Assert.IsTrue(Contains(actualResult, item2));
        Assert.IsTrue(Contains(actualResult, item3));
        Assert.IsTrue(Contains(actualResult, item4));
    }

    protected void AssertCollectionsAreEqual(IEnumerable<GenericParameterHelper> expected, IEnumerable<GenericParameterHelper> actual)
    {
        if (expected is not ICollection expectedItemsAsCollection)
        {
            expectedItemsAsCollection = expected.ToList();
        }
        if (actual is not ICollection actualAsCollection)
        {
            actualAsCollection = actual.ToList();
        }
        AssertCollectionsAreEqual(expectedItemsAsCollection, actualAsCollection);
    }

    protected abstract void AssertCollectionsAreEqual(ICollection expected, ICollection actual);
}
