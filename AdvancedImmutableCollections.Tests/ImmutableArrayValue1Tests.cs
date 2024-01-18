using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections.Tests;

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1Tests : ImmutableListTestsBase<ImmutableArrayValue<GenericParameterHelper>>
{
    protected override ImmutableArrayValue<GenericParameterHelper> GetTestObject() => new ImmutableArrayValue<GenericParameterHelper>();
    protected override ImmutableArrayValue<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableArrayValue<GenericParameterHelper>(initialItems.ToImmutableArray());

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableArrayValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableArrayValue<GenericParameterHelper> collection) => collection.Clear();
}

/// <summary>
/// Verifies <see cref="ImmutableArrayValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableArrayValue1_ExplicitIImmutableList_Tests : ExplicitImmutableListTestsBase<IImmutableList<GenericParameterHelper>>
{
    protected override IImmutableList<GenericParameterHelper> GetTestObject() => new ImmutableArrayValue<GenericParameterHelper>();
    protected override IImmutableList<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableArrayValue<GenericParameterHelper>(initialItems.ToImmutableArray());
}

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/>
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1Tests : ImmutableSetTestsBase<ImmutableHashSetValue<GenericParameterHelper>>
{
    protected override ImmutableHashSetValue<GenericParameterHelper> GetTestObject() => new ImmutableHashSetValue<GenericParameterHelper>();
    protected override ImmutableHashSetValue<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());

    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(ImmutableHashSetValue<GenericParameterHelper> collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(ImmutableHashSetValue<GenericParameterHelper> collection) => collection.Clear();
}

/// <summary>
/// Verifies <see cref="ImmutableHashSetValue{T}"/> using <see cref="IImmutableList{T}"/> interface explicitly
/// </summary>
[TestClass]
public sealed class ImmutableHashSetValue1_ExplicitIImmutableList_Tests : ExplicitImmutableSetTestsBase<IImmutableSet<GenericParameterHelper>>
{
    protected override IImmutableSet<GenericParameterHelper> GetTestObject() => new ImmutableHashSetValue<GenericParameterHelper>();
    protected override IImmutableSet<GenericParameterHelper> GetTestObject(params GenericParameterHelper[] initialItems) => new ImmutableHashSetValue<GenericParameterHelper>(initialItems.ToImmutableArray());
}

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

    private static void AssertCollectionsAreEqual(IEnumerable<GenericParameterHelper> expected, IEnumerable<GenericParameterHelper> actual)
    {
        if (expected is not ICollection expectedItemsAsCollection)
        {
            expectedItemsAsCollection = expected.ToList();
        }
        if (actual is not ICollection actualAsCollection)
        {
            actualAsCollection = actual.ToList();
        }
        CollectionAssert.AreEquivalent(expectedItemsAsCollection, actualAsCollection);
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
            Assert.IsTrue(testObject.Contains(itemToRemove));
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
        Assert.IsTrue(testObject.Contains(item0));
        Assert.IsFalse(testObject.Contains(item1));
        Assert.IsTrue(testObject.Contains(item2));

        testObject = GetTestObject();
        Assert.IsFalse(testObject.Contains(item0));
    }

    [TestMethod]
    public void Contains_Empty_Test()
    {
        var item = new GenericParameterHelper(0);
        var testObject = GetTestObject();
        Assert.IsFalse(testObject.Contains(item));
    }
}

public abstract class ExplicitImmutableListTestsBase<TTestObject> : ImmutableListTestsBase<TTestObject>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => collection.Add(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => collection.Remove(item);

    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();
}

public abstract class ImmutableListTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, List<GenericParameterHelper>>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override List<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);
}

public abstract class ImmutableSetTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, HashSet<GenericParameterHelper>>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override HashSet<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);
}

public abstract class ExplicitImmutableSetTestsBase<TTestObject> : ImmutableSetTestsBase<TTestObject>
    where TTestObject : IImmutableSet<GenericParameterHelper>
{
    protected sealed override IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item) => ((IImmutableSet<GenericParameterHelper>)collection).Add(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item) => ((IImmutableSet<GenericParameterHelper>)collection).Remove(item);
    protected override IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection) => collection.Clear();
}
