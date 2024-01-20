using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections;
using System.Collections.Immutable;

namespace AdvancedImmutableCollections;

public abstract class ImmutableListTestsBase<TTestObject> : ImmutableCollectionTestsBase<TTestObject, List<GenericParameterHelper>>
    where TTestObject : IImmutableList<GenericParameterHelper>
{
    protected sealed override List<GenericParameterHelper> GetMutableCollection(params GenericParameterHelper[] initialItems) => new(initialItems);

    protected abstract int IndexOf(TTestObject collection, GenericParameterHelper item, int index, int count, IEqualityComparer<GenericParameterHelper>? equalityComparer);

    protected abstract int LastIndexOf(TTestObject collection, GenericParameterHelper item, int index, int count, IEqualityComparer<GenericParameterHelper>? equalityComparer);

    protected abstract IImmutableList<GenericParameterHelper> Insert(TTestObject collection, int index, GenericParameterHelper item);

    protected abstract IImmutableList<GenericParameterHelper> InsertRange(TTestObject collection, int index, params GenericParameterHelper[] items);

    protected abstract IImmutableList<GenericParameterHelper> Remove(TTestObject collection, GenericParameterHelper itemToRemove, IEqualityComparer<GenericParameterHelper>? equalityComparer);

    protected abstract IImmutableList<GenericParameterHelper> RemoveAt(TTestObject collection, int index);

    protected abstract IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, int start, int count);

    protected abstract IImmutableList<GenericParameterHelper> RemoveRange(TTestObject collection, IEnumerable<GenericParameterHelper> items, IEqualityComparer<GenericParameterHelper>? equalityComparer);

    protected abstract IImmutableList<GenericParameterHelper> RemoveAll(TTestObject collection, Predicate<GenericParameterHelper> predicate);

    protected abstract GenericParameterHelper GetItem(TTestObject collection, int index);
    protected abstract IImmutableList<GenericParameterHelper> SetItem(TTestObject collection, int index, GenericParameterHelper item);

    protected abstract IImmutableList<GenericParameterHelper> Replace(TTestObject collection, GenericParameterHelper oldValue, GenericParameterHelper newValue, IEqualityComparer<GenericParameterHelper>? equalityComparer);


    [TestMethod]
    public void IndexOfTest()
    {
        var item0_0 = new GenericParameterHelper(0);
        var item1_1 = new GenericParameterHelper(1);
        var item2_2 = new GenericParameterHelper(2);
        var item3_3 = new GenericParameterHelper(3);
        var item4_0 = new GenericParameterHelper(0);
        var item5_1 = new GenericParameterHelper(1);
        var item6_0 = new GenericParameterHelper(0);
        var testObject = GetTestObject(item0_0, item1_1, item2_2, item3_3, item4_0, item5_1, item6_0);

        IndexOfTest(item6_0, 0, 1, 0);
        IndexOfTest(item0_0, 1, 1, -1);
        IndexOfTest(item0_0, 1, 6, 4);
        IndexOfTest(item0_0, 5, 2, 6);
        IndexOfTest(item2_2, 3, 4, -1);
        IndexOfTest(item2_2, 0, 2, -1);
        IndexOfTest(item2_2, 0, 3, 2);
        IndexOfTest(item2_2, 1, 2, 2);
        IndexOfTest(item2_2, 2, 1, 2);
        IndexOfTest(item2_2, 2, 0, -1);
        IndexOfArgumentOutOfRangeTest(item2_2, 2, -1);
        IndexOfTest(new GenericParameterHelper(7), 0, 7, -1);
        IndexOfArgumentOutOfRangeTest(new GenericParameterHelper(7), 0, 8);
        IndexOfArgumentOutOfRangeTest(item0_0, -1, 2);
        IndexOfArgumentOutOfRangeTest(item0_0, 0, -1);

        IndexOfTest(item6_0, 0, 7, 0, EqualityComparer<GenericParameterHelper>.Default);
        IndexOfTest(item6_0, 0, 7, 6, ReferenceEqualityComparer.Instance);

        testObject = GetTestObject();
        IndexOfTest(item0_0, 0, 0, -1);
        IndexOfArgumentOutOfRangeTest(item0_0, 0, 1);
        IndexOfArgumentOutOfRangeTest(item0_0, 0, -1);
        IndexOfArgumentOutOfRangeTest(item0_0, 1, 0);
        IndexOfArgumentOutOfRangeTest(item0_0, -1, 0);

        testObject = default;
        if (testObject is not null)
        {
            IndexOfTest(item0_0, 0, 0, -1);
            IndexOfArgumentOutOfRangeTest(item0_0, 0, 1);
            IndexOfArgumentOutOfRangeTest(item0_0, 0, -1);
            IndexOfArgumentOutOfRangeTest(item0_0, 1, 0);
            IndexOfArgumentOutOfRangeTest(item0_0, -1, 0);
            IndexOfArgumentOutOfRangeTest(item0_0, -1, 1);
        }

        void IndexOfTest(GenericParameterHelper item, int index, int count, int expexted, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            var actual = IndexOf(testObject, item, index, count, equalityComparer);
            Assert.AreEqual(expexted, actual);
        }

        void IndexOfArgumentOutOfRangeTest(GenericParameterHelper item, int index, int count, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => IndexOf(testObject, item, index, count, equalityComparer));
        }
    }

    [TestMethod]
    public void LastIndexOfTest()
    {
        var item0_0 = new GenericParameterHelper(0);
        var item1_1 = new GenericParameterHelper(1);
        var item2_2 = new GenericParameterHelper(2);
        var item3_3 = new GenericParameterHelper(3);
        var item4_0 = new GenericParameterHelper(0);
        var item5_1 = new GenericParameterHelper(1);
        var item6_0 = new GenericParameterHelper(0);
        var testObject = GetTestObject(item0_0, item1_1, item2_2, item3_3, item4_0, item5_1, item6_0);

        LastIndexOfTest(item0_0, 6, 7, 6);
        LastIndexOfTest(item0_0, 5, 2, 4);
        LastIndexOfTest(item4_0, 5, 1, -1);
        LastIndexOfTest(item4_0, 4, 0, -1);
        LastIndexOfTest(item1_1, 5, 5, 5);
        LastIndexOfTest(item1_1, 4, 3, -1);
        LastIndexOfTest(item6_0, 1, 2, 0);
        LastIndexOfTest(item6_0, 1, 1, -1);
        LastIndexOfTest(item6_0, 0, 1, 0);
        LastIndexOfTest(item6_0, 0, 0, -1);

        LastIndexOfTest(item0_0, 6, 6, 6, EqualityComparer<GenericParameterHelper>.Default);
        LastIndexOfTest(item1_1, 4, 4, 1, EqualityComparer<GenericParameterHelper>.Default);

        LastIndexOfTest(item0_0, 6, 7, 0, ReferenceEqualityComparer.Instance);
        LastIndexOfTest(item0_0, 6, 5, -1, ReferenceEqualityComparer.Instance);
        LastIndexOfTest(item1_1, 5, 5, 1, ReferenceEqualityComparer.Instance);
        LastIndexOfTest(item6_0, 0, 1, -1, ReferenceEqualityComparer.Instance);

        LastIndexOfArgumentOutOfRangeTest(item0_0, 8, 1);
        LastIndexOfArgumentOutOfRangeTest(item0_0, -1, 1);
        LastIndexOfArgumentOutOfRangeTest(item0_0, 6, -1);

        testObject = GetTestObject([]);
        LastIndexOfTest(item0_0, 0, 0, -1);
        LastIndexOfArgumentOutOfRangeTest(item0_0, 0, 1);
        LastIndexOfArgumentOutOfRangeTest(item0_0, 0, -2);
        LastIndexOfArgumentOutOfRangeTest(item0_0, 1, 0);
        LastIndexOfArgumentOutOfRangeTest(item0_0, -1, 0);

        testObject = default;
        if (testObject is not null)
        {
            LastIndexOfTest(item0_0, 0, 0, -1);
            LastIndexOfArgumentOutOfRangeTest(item0_0, 0, 1);
            LastIndexOfArgumentOutOfRangeTest(item0_0, 0, -1);
            LastIndexOfArgumentOutOfRangeTest(item0_0, 1, 0);
            LastIndexOfArgumentOutOfRangeTest(item0_0, -1, 0);
        }

        void LastIndexOfTest(GenericParameterHelper item, int index, int count, int expexted, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            var actual = LastIndexOf(testObject, item, index, count, equalityComparer);
            Assert.AreEqual(expexted, actual);
        }

        void LastIndexOfArgumentOutOfRangeTest(GenericParameterHelper item, int index, int count, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => LastIndexOf(testObject, item, index, count, equalityComparer));
        }
    }

    [TestMethod]
    public void InsertTest()
    {
        var testObject = GetTestObject();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);

        InsertIndexOutOfRangeTest(item1, 1);
        InsertIndexOutOfRangeTest(item0, -1);
        InsertTest(item0, 0, [item0]);
        InsertTest(item1, 0, [item1, item0]);
        InsertTest(item2, 2, [item1, item0, item2]);
        InsertTest(item3, 1, [item1, item3, item0, item2]);
        InsertIndexOutOfRangeTest(new GenericParameterHelper(4), 5);

        testObject = default;
        if (testObject is not null)
        {
            InsertIndexOutOfRangeTest(item1, 1);
            InsertIndexOutOfRangeTest(item1, -1);
            testObject = default!;
            InsertTest(item1, 0, [item1]);
        }

        void InsertTest(GenericParameterHelper item, int index, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = Insert(testObject, index, item);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }

        void InsertIndexOutOfRangeTest(GenericParameterHelper item, int index)
        {
            var itemsBefore = testObject.ToList();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => Insert(testObject, index, item));
            AssertCollectionsAreEqual(itemsBefore, testObject);
        }
    }

    [TestMethod]
    public void InsertRangeTest()
    {
        var testObject = GetTestObject();
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);

        InsertRangeIndexOutOfRangeTest(-1, [item0, item1]);
        InsertRangeIndexOutOfRangeTest(1, [item1]);
        InsertRangeTest(0, [item0, item1], [item0, item1]);
        InsertRangeIndexOutOfRangeTest(3, [item2]);
        InsertRangeTest(2, [item2, item3], [item0, item1, item2, item3]);
        InsertRangeTest(1, [item4, item5], [item0, item4, item5, item1, item2, item3]);
        InsertRangeTest(6, [item6], [item0, item4, item5, item1, item2, item3, item6]);
        InsertRangeTest(5, [item7, item8, item9], [item0, item4, item5, item1, item2, item7, item8, item9, item3, item6]);
        InsertRangeTest(5, [item4, item9], [item0, item4, item5, item1, item2, item4, item9, item7, item8, item9, item3, item6]);

        testObject = default;
        if (testObject is not null)
        {
            InsertRangeTest(0, [item0], [item0]);
            testObject = default!;
            InsertRangeIndexOutOfRangeTest(1, [item0]);
            InsertRangeIndexOutOfRangeTest(-1, [item0]);
        }

        void InsertRangeTest(int index, GenericParameterHelper[] items, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = InsertRange(testObject, index, items);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }

        void InsertRangeIndexOutOfRangeTest(int index, GenericParameterHelper[] items)
        {
            var itemsBefore = testObject.ToList();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => InsertRange(testObject, index, items));
            AssertCollectionsAreEqual(itemsBefore, testObject);
        }
    }

    [TestMethod]
    public void RemoveAtTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var testObject = GetTestObject(item0, item1, item2, item3, item4);

        RemoveAtIndexOutOfRangeTest(-2);
        RemoveAtIndexOutOfRangeTest(5);
        RemoveAtTest(4, [item0, item1, item2, item3]);
        RemoveAtTest(2, [item0, item1, item3]);
        RemoveAtTest(0, [item1, item3]);
        RemoveAtTest(1, [item1]);
        RemoveAtTest(0, []);
        RemoveAtIndexOutOfRangeTest(0);
        RemoveAtIndexOutOfRangeTest(-1);

        void RemoveAtTest(int index, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveAt(testObject, index);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }

        void RemoveAtIndexOutOfRangeTest(int index)
        {
            var itemsBefore = testObject.ToList();
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => RemoveAt(testObject, index));
            AssertCollectionsAreEqual(itemsBefore, testObject);
        }
    }

    /// <summary>
    /// Verifies removing a range using start index and count
    /// </summary>
    [TestMethod]
    public void RemoveRange_int_int_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var testObject = GetTestObject(item0, item1, item2, item3, item4, item5, item6, item7, item8, item9);

        RemoveRangeTest(0, 0, [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest(0, 1, [item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest(7, 2, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveRangeTest(2, 3, [item1, item2, item6, item7]);
        RemoveRangeTest(1, 1, [item1, item6, item7]);
        RemoveRangeTest(0, 3, []);

        void RemoveRangeTest(int start, int count, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, start, count);
            AssertCollectionsAreEqual(expected, actual);
            AssertCollectionsAreEqual(itemsBefore, testObject);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    /// <summary>
    /// Verifies removing a range using a predicate: RemoveAll(Predicate&lt;T&gt; match)
    /// </summary>
    [TestMethod]
    public void RemoveAllTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10, new GenericParameterHelper(10));

        RemoveAllTest(it => it.Data == 10, [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveAllTest(it => it.Data > 7, [item0, item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data <= 0, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data < 1, [item1, item2, item3, item4, item5, item6, item7]);
        RemoveAllTest(it => it.Data % 2 == 0, [item1, item3, item5, item7]);
        RemoveAllTest(it => it.Data % 2 == 0, [item1, item3, item5, item7]);
        RemoveAllTest(it => it.Data is 3 or 5, [item1, item7]);
        RemoveAllTest(it => it.Data is 1 or 7, []);
        RemoveAllTest(PredicateNotExpected, []);


        void RemoveAllTest(Predicate<GenericParameterHelper> predicate, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveAll(testObject, predicate);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }

        bool PredicateNotExpected(GenericParameterHelper item)
        {
            Assert.Fail("Predicate should not be called");
            return false;
        }
    }

    /// <summary>
    /// Verifies removing a range using another collection and an equality comparer: RemoveRange(IEnumerable&lt;T&gt; items, IEqualityComparer&lt;T&gt;? equalityComparer)
    /// </summary>
    [TestMethod]
    public void RemoveRange_IEnumerable_IEqualityComparer_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var item10_2 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item10_2, item4, item10, item5, item6, item7, item8, item9, item10);

        RemoveRangeTest([item10, item10], [item0, item1, item2, item3, item4, item10, item5, item6, item7, item8, item9, item10]);
        RemoveRangeTest([item10], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item10_2], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9]);
        RemoveRangeTest([item8, item3, item4], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([item8], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([item4, item7, item5], [item0, item1, item2, item6, item9]);
        RemoveRangeTest([], [item0, item1, item2, item6, item9], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item0, item9], [item1, item2, item6]);
        RemoveRangeTest([item1, item9], [item2, item6], EqualityComparer<GenericParameterHelper>.Default);
        RemoveRangeTest([item6, item2], []);
        RemoveRangeTest([item6, item2], []);
        RemoveRangeTest([], []);

        void RemoveRangeTest(IEnumerable<GenericParameterHelper> items, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, items, equalityComparer);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    /// <summary>
    /// Verifies removing a range using another collection and an equality comparer: RemoveRange(IEnumerable&lt;T&gt; items, IEqualityComparer&lt;T&gt;? equalityComparer)
    /// </summary>
    [TestMethod]
    public void RemoveRange_IEnumerable_IEqualityComparer_Test2()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var item5 = new GenericParameterHelper(5);
        var item6 = new GenericParameterHelper(6);
        var item7 = new GenericParameterHelper(7);
        var item8 = new GenericParameterHelper(8);
        var item9 = new GenericParameterHelper(9);
        var item10 = new GenericParameterHelper(10);
        var item10_2 = new GenericParameterHelper(10);
        var testObject = GetTestObject(item10, item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10_2, item10, item10_2, item10);

        RemoveRangeTest([item10, item10], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10_2, item10_2, item10]);
        RemoveRangeTest([item10_2, item10_2, item10_2], [item0, item1, item2, item3, item4, item5, item6, item7, item8, item9, item10]);
        RemoveRangeTest([item8, item10, new GenericParameterHelper(3), new GenericParameterHelper(4)], [item0, item1, item2, item3, item4, item5, item6, item7, item9]);
        RemoveRangeTest([item3, item4], [item0, item1, item2, item5, item6, item7, item9]);
        RemoveRangeTest([new GenericParameterHelper(0), new GenericParameterHelper(1), item1, item2, item5, item6, item7, new GenericParameterHelper(9)], [item0, item9]);
        RemoveRangeTest([item0, item9, new GenericParameterHelper(0)], []);
        RemoveRangeTest([item0, new GenericParameterHelper(0)], []);

        void RemoveRangeTest(IEnumerable<GenericParameterHelper> items, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = RemoveRange(testObject, items, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void GetItemTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var testObject = GetTestObject(item0, item1, item2);

        GetItemTest(0, item0);
        GetItemTest(1, item1);
        GetItemTest(2, item2);
        GetItemIndexOfRangeTest(3);
        GetItemIndexOfRangeTest(-1);
        GetItemIndexOfRangeTest(int.MinValue);
        GetItemIndexOfRangeTest(int.MaxValue);

        testObject = GetTestObject();
        GetItemIndexOfRangeTest(0);
        GetItemIndexOfRangeTest(-1);
        GetItemIndexOfRangeTest(1);

        void GetItemTest(int index, GenericParameterHelper expected)
        {
            var actual = GetItem(testObject, index);
            Assert.AreEqual(expected, actual);
        }

        void GetItemIndexOfRangeTest(int index)
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => GetItem(testObject, index));
        }
    }

    [TestMethod]
    public void SetItemTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var testObject = GetTestObject(item0, item1, item2);

        SetItemTest(0, item3, [item3, item1, item2]);
        SetItemTest(2, item4, [item3, item1, item4]);
        SetItemTest(1, item4, [item3, item4, item4]);

        void SetItemTest(int index, GenericParameterHelper item, GenericParameterHelper[] expected)
        {
            var itemsBefore = testObject.ToList();
            var actual = SetItem(testObject, index, item);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void ReplaceTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);
        var item2b = new GenericParameterHelper(2);
        var item2c = new GenericParameterHelper(2);
        var item3 = new GenericParameterHelper(3);
        var item4 = new GenericParameterHelper(4);
        var testObject = GetTestObject(item0, item1, item2);

        ReplaceTest(item1, item2b, [item0, item2b, item2]);
        ReplaceTest(item2, item2c, [item0, item2b, item2c], ReferenceEqualityComparer.Instance);
        ReplaceTest(item2, item2, [item0, item2, item2c], EqualityComparer<GenericParameterHelper>.Default);
        ReplaceTest(item2c, item2b, [item0, item2b, item2c]);
        Assert.ThrowsException<ArgumentException>(() => Replace(testObject, new GenericParameterHelper(0), item3, ReferenceEqualityComparer.Instance));
        AssertCollectionsAreEqual(testObject, [item0, item2b, item2c]);
        ReplaceTest(item0, item3, [item3, item2b, item2c], ReferenceEqualityComparer.Instance);

        void ReplaceTest(GenericParameterHelper item, GenericParameterHelper newItem, GenericParameterHelper[] expected, IEqualityComparer<GenericParameterHelper>? equalityComparer = null)
        {
            var itemsBefore = testObject.ToList();
            var actual = Replace(testObject, item, newItem, equalityComparer);
            AssertCollectionsAreEqual(expected, actual, ReferenceEqualityComparer.Instance);
            AssertCollectionsAreEqual(itemsBefore, testObject, ReferenceEqualityComparer.Instance);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }

    [TestMethod]
    public void GetEnumeratorTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        GetEnumeratorTest([]);
        GetEnumeratorTest([item0]);
        GetEnumeratorTest([item0, item1]);
        GetEnumeratorTest([item0, item1, item2]);
        GetEnumeratorTest([item0, item1, item0]);

        // test with default(TTestObject) because internal variable will be null
        if (default(TTestObject) is { } testObject2)
        {
            GetEnumeratorTestCore(testObject2, Enumerable.Empty<GenericParameterHelper>());
        }

        void GetEnumeratorTest(GenericParameterHelper[] items)
        {
            var testObject = GetTestObject(items);
            GetEnumeratorTestCore(testObject, items);
        }

        void GetEnumeratorTestCore(TTestObject testObject, IEnumerable<GenericParameterHelper> expectedItems)
        {
            IEnumerator<GenericParameterHelper> expected = expectedItems.GetEnumerator();
            IEnumerator<GenericParameterHelper> actual = GetEnumerator(testObject);
            Assert.IsNotNull(actual);
            
            while (expected.MoveNext())
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(expected.Current, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    [TestMethod]
    public void IEnumerable_GetEnumeratorTest()
    {
        var item0 = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item2 = new GenericParameterHelper(2);

        GetEnumeratorTest([]);
        GetEnumeratorTest([item0]);
        GetEnumeratorTest([item0, item1]);
        GetEnumeratorTest([item0, item1, item2]);
        GetEnumeratorTest([item0, item0, item1, item2]);

        // test with default(TTestObject) because internal variable will be null
        if (default(TTestObject) is { } testObject2)
        {
            GetEnumeratorTestCore(testObject2, Enumerable.Empty<GenericParameterHelper>());
        }

        void GetEnumeratorTest(GenericParameterHelper[] items)
        {
            var testObject = GetTestObject(items);
            GetEnumeratorTestCore(testObject, items);
        }

        void GetEnumeratorTestCore(IEnumerable testObject, IEnumerable expectedItems)
        {
            IEnumerator expected = expectedItems.GetEnumerator();
            IEnumerator actual = testObject.GetEnumerator();
            Assert.IsNotNull(actual);
            while (expected.MoveNext())
            {
                Assert.IsTrue(actual.MoveNext());
                Assert.AreEqual(expected.Current, actual.Current);
            }
            Assert.IsFalse(actual.MoveNext());
        }
    }

    [TestMethod]
    public void IImmutableList_Remove_Test()
    {
        var item0 = new GenericParameterHelper(0);
        var item0b = new GenericParameterHelper(0);
        var item0c = new GenericParameterHelper(0);
        var item1 = new GenericParameterHelper(1);
        var item1b = new GenericParameterHelper(1);

        var testObject = GetTestObject([item0, item1, item0b, item1b, item0c]);
        RemoveTest(new GenericParameterHelper(1), [item0, item1, item0b, item1b, item0c], ReferenceEqualityComparer.Instance);
        RemoveTest(item0c, [item0, item1, item0b, item1b], ReferenceEqualityComparer.Instance);
        RemoveTest(item0c, [item1, item0b, item1b], EqualityComparer<GenericParameterHelper>.Default);
        RemoveTest(item1b, [item0b, item1b], null);
        RemoveTest(item0, [item1b], null);
        RemoveTest(item1, [item1b], ReferenceEqualityComparer.Instance);
        RemoveTest(item1, [], EqualityComparer<GenericParameterHelper>.Default);
        RemoveTest(item1, [], EqualityComparer<GenericParameterHelper>.Default);
        RemoveTest(item1, [], ReferenceEqualityComparer.Instance);

        testObject = default;
        if (testObject is not null)
        {
            RemoveTest(item0, [], null);
            RemoveTest(item0, [], ReferenceEqualityComparer.Instance); 
        }

        void RemoveTest(GenericParameterHelper itemToRemove, GenericParameterHelper[] expectedItems, IEqualityComparer<GenericParameterHelper>? equalityComparer)
        {
            var itemsBefore = testObject.ToList();
            var actual = Remove(testObject, itemToRemove, equalityComparer);
            AssertCollectionsAreEqual(itemsBefore, testObject);
            AssertCollectionsAreEqual(expectedItems, actual);

            if (actual is not TTestObject actualAsT)
            {
                Assert.Fail($"actual result is not of expected type");
                return;
            }
            testObject = actualAsT;
        }
    }
}