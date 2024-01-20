using System.Collections;
using System.Collections.Immutable;
using AdvancedImmutableCollections.Tests.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AdvancedImmutableCollections;

/// <summary>
/// Base class for tests of immutable collections.
/// </summary>
/// <typeparam name="TTestObject">The type that is tested by this test class</typeparam>
/// <typeparam name="TMutable">
/// Type of a mutable collection that has equal semantics as the <typeparamref name="TTestObject"/> type.
/// E.g. if <typeparamref name="TTestObject"/> is a set type, this must also be a set type.
/// </typeparam>
public abstract partial class ImmutableCollectionTestsBase<TTestObject, TMutable>
    where TTestObject : IReadOnlyCollection<GenericParameterHelper>
    where TMutable : ICollection<GenericParameterHelper>
{
    /// <summary>
    /// Gets the <see langword="default"/> value of the tested type.
    /// </summary>
    protected virtual TTestObject? DefaultValue => (TTestObject?)GetDefaultValue<GenericParameterHelper>();

    /// <summary>
    /// Gets the <see langword="default"/> value of the tested type with specified type parameter.
    /// </summary>
    /// <typeparam name="T">Type of the items in the collection</typeparam>
    /// <returns></returns>
    protected abstract IReadOnlyCollection<T>? GetDefaultValue<T>();

    /// <summary>
    /// Creates an empty instance of <typeparamref name="TTestObject"/>.
    /// </summary>
    /// <returns>An empty instance. If a value type is tested, the result might be different than <see cref="DefaultValue"/>.</returns>
    internal protected abstract TTestObject CreateInstance();

    /// <summary>
    /// Creates an instance of the tested type with the specified items of type <see cref="GenericParameterHelper"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="initialItems">The items that are contained in the collection</param>
    /// <returns></returns>
    internal protected virtual TTestObject CreateInstance(params GenericParameterHelper[] initialItems)
        => (TTestObject)CreateInstance<GenericParameterHelper>(initialItems);

    /// <summary>
    /// Creates an instance of the tested type with the specified items of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="initialItems">The items that are contained in the collection</param>
    /// <returns></returns>
    protected abstract IReadOnlyCollection<T> CreateInstance<T>(params T[] initialItems);

    /// <summary>
    /// Gets a mutable collection with equal semantics of the tested type
    /// </summary>
    /// <param name="initialItems"></param>
    /// <returns></returns>
    protected abstract TMutable GetMutableCollection(params GenericParameterHelper[] initialItems);

    protected abstract IReadOnlyCollection<GenericParameterHelper> Add(TTestObject collection, GenericParameterHelper item);
    protected abstract IReadOnlyCollection<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper item);
    protected abstract IReadOnlyCollection<GenericParameterHelper> Clear(TTestObject collection);
    protected abstract IReadOnlyCollection<GenericParameterHelper> AddRange(TTestObject collection, params GenericParameterHelper[] newItems);
    protected abstract bool Contains(TTestObject collection, GenericParameterHelper item);
    protected abstract IEnumerator<GenericParameterHelper> GetEnumerator(TTestObject collection);

    protected abstract IEqualityTestStrategy EqualityTestStrategy { get; }

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
        var testObject = CreateInstance();
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
        var expectedItems = GetMutableCollection(items.ToArray());
        var testObject = CreateInstance(item0, item1, item2);
        AssertCollectionsAreEqual(expectedItems, testObject);

        RemoveTest(new GenericParameterHelper(3), false);

        foreach (var itemToRemove in items)
        {
            RemoveTest(itemToRemove, true);
        }

        RemoveTest(item0, false);

        testObject = DefaultValue;
        if (testObject is not null)
        {
            RemoveTest(item0, false);
        }

        void RemoveTest(GenericParameterHelper itemToRemove, bool expectRemoved)
        {
            var itemsBefore = testObject.ToList();
            var actualResult = Remove(testObject, itemToRemove);
            if (expectRemoved)
            {
                Assert.AreNotSame(testObject, actualResult);
                Assert.IsTrue(Contains(testObject, itemToRemove));
            }
            else if (DefaultValue is not null)
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
        var testObject = CreateInstance(item0, item1);

        var itemsBefore = testObject.ToList();
        var actualResult = Clear(testObject);
        Assert.AreEqual(0, actualResult.Count);
        Assert.IsFalse(actualResult.Contains(item0));
        Assert.IsFalse(actualResult.Contains(item1));
        Assert.AreNotSame(testObject, actualResult);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = GetMutableCollection();
        AssertCollectionsAreEqual(expected, actualResult);
    }

    [TestMethod]
    public void Clear_Empty_Test()
    {
        var testObject = CreateInstance();
        var actual = Clear(testObject);
        Assert.AreEqual(0, actual.Count);
        AssertCollectionsAreEqual(testObject, actual);
    }

    [TestMethod]
    public void ContainsTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var testObject = CreateInstance(item0, item2);
        Assert.IsTrue(Contains(testObject, item0));
        Assert.IsFalse(Contains(testObject, item1));
        Assert.IsTrue(Contains(testObject, item2));

        testObject = CreateInstance();
        Assert.IsFalse(testObject.Contains(item0));
    }

    [TestMethod]
    public void Contains_Empty_Test()
    {
        var item = new GenericParameterHelper(0);
        var testObject = CreateInstance();
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

        var testObject = CreateInstance(item0, item1);

        var itemsBefore = testObject.ToList();
        var actualResult = AddRange(testObject, item2, item3, item4);
        AssertCollectionsAreEqual(itemsBefore, testObject);
        var expected = new GenericParameterHelper[] { item0, item1, item2, item3, item4 };
        AssertCollectionsAreEqual(expected, actualResult);
        Assert.IsTrue(Contains(actualResult, item2));
        Assert.IsTrue(Contains(actualResult, item3));
        Assert.IsTrue(Contains(actualResult, item4));
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

        if (DefaultValue is not null)
        {
            var actual = DefaultValue.Count;
            Assert.AreEqual(0, actual);
        }

        void CountTest(GenericParameterHelper[] items)
        {
            var testObject = CreateInstance(items);
            Assert.AreEqual(items.Length, testObject.Count);
        }
    }

    [TestMethod]
    public void EqualsTest()
    {
        EqualityTestStrategy.EqualsTest(this);
    }

    [TestMethod]
    public void GetHashCodeTest()
    {
        EqualityTestStrategy.GetHashCodeTest(this);
    }

    protected virtual void AssertCollectionsAreEqual<T>(IEnumerable<T> expected, IEnumerable<T> actual, IEqualityComparer<T>? itemComparer = null)
        => CollectionAssert.That.AreEqual(expected, actual, itemComparer);
}
